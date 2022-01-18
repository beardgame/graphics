using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Bearded.Graphics.Content
{
    public sealed class Bitmap<TPixel>
        where TPixel : struct
    {
        internal readonly TPixel[] Pixels;
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

            Pixels = pixels;
            Width = width;
            Height = height;
        }

        public Bitmap<TPixelTo> To<TPixelTo>(Func<TPixel, TPixelTo> convertPixel)
            where TPixelTo : struct
        {
            var pixelCount = Width * Height;
            var newPixels = new TPixelTo[pixelCount];

            for (var i = 0; i < pixelCount; i++)
            {
                newPixels[i] = convertPixel(Pixels[i]);
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

            copyBytes(bitmap, Pixels, CopyMode.ArgbToBitmap, (ptr, arrayPixels, count) =>
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
    }
}
