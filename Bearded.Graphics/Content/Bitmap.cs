using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Content
{
    public sealed class Bitmap<TPixel> : IEnumerable<(TPixel Pixel, Vector2i Xy)>
        where TPixel : struct
    {
        private readonly TPixel[] pixels;
        public int Width { get; }
        public int Height { get; }

        public Bitmap(int width, int height)
            : this(new TPixel[width * height], width, height)
        {
        }

        internal Bitmap(TPixel[] pixels, int width, int height)
        {
            if (pixels.Length != width * height)
                throw new ArgumentException("Bitmap array must have width * height pixels");

            this.pixels = pixels;
            Width = width;
            Height = height;
        }

        public TPixel this[int x, int y]
        {
            get => pixels[index(x, y)];
            set => pixels[index(x, y)] = value;
        }

        public TPixel this[Vector2i xy]
        {
            get => pixels[index(xy.X, xy.Y)];
            set => pixels[index(xy.X, xy.Y)] = value;
        }

        private int index(int x, int y) => y * Width + x;
        private Vector2i xy(int index) => new Vector2i(index % Width, index / Width);

        public Bitmap<TPixelTo> To<TPixelTo>(Func<TPixel, TPixelTo> convertPixel)
            where TPixelTo : struct
        {
            var pixelCount = Width * Height;
            var newPixels = new TPixelTo[pixelCount];

            for (var i = 0; i < pixelCount; i++)
            {
                newPixels[i] = convertPixel(pixels[i]);
            }

            return new Bitmap<TPixelTo>(newPixels, Width, Height);
        }

        public Bitmap<TPixelTo> To<TPixelTo>(Func<TPixel, Vector2i, TPixelTo> convertPixel)
            where TPixelTo : struct
        {
            var pixelCount = Width * Height;
            var newPixels = new TPixelTo[pixelCount];

            for (var i = 0; i < pixelCount; i++)
            {
                newPixels[i] = convertPixel(pixels[i], xy(i));
            }

            return new Bitmap<TPixelTo>(newPixels, Width, Height);
        }

        public static Bitmap<TPixel> From(Bitmap bitmap, Func<Color, TPixel> convert)
        {
            var pixels = new TPixel[bitmap.Width * bitmap.Height];

            copyBytes(bitmap, pixels, CopyMode.BitmapToArgb, (ptr, arrayPixels, count) =>
            {
                unsafe
                {
                    var bitmapPixels = (uint*)ptr;

                    for (var i = 0; i < count; i++)
                        arrayPixels[i] = convert(new Color(bitmapPixels[i]));
                }
            });

            return new Bitmap<TPixel>(pixels, bitmap.Width, bitmap.Height);
        }

        public Bitmap ToSystemBitmap(Func<TPixel, Color> convert)
        {
            var bitmap = new Bitmap(Width, Height);

            copyBytes(bitmap, pixels, CopyMode.ArgbToBitmap, (ptr, arrayPixels, count) =>
            {
                unsafe
                {
                    var bitmapPixels = (uint*)ptr;

                    for (var i = 0; i < count; i++)
                        bitmapPixels[i] = convert(arrayPixels[i]).ARGB;
                }
            });

            return bitmap;
        }

        enum CopyMode
        {
            BitmapToArgb,
            ArgbToBitmap
        }

        private static void copyBytes(Bitmap bitmap, TPixel[] pixels, CopyMode mode, Action<IntPtr, TPixel[], int> copy)
        {
            var data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                mode == CopyMode.BitmapToArgb ? ImageLockMode.ReadOnly : ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb
            );

            var pixelCount = bitmap.Width * bitmap.Height;

            copy(data.Scan0, pixels, pixelCount);

            bitmap.UnlockBits(data);
        }

        public IEnumerator<(TPixel, Vector2i)> GetEnumerator()
        {
            var pixelCount = Width * Height;

            for (var i = 0; i < pixelCount; i++)
            {
                yield return (pixels[i], xy(i));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
