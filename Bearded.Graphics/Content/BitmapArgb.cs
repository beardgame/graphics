using System;
using System.Drawing;
using System.Drawing.Imaging;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Bearded.Graphics.Content
{
    public sealed class BitmapArgb
    {
        private readonly ColorArgb[] argb;
        public int Width { get; }
        public int Height { get; }

        public BitmapArgb(int width, int height)
            : this(new ColorArgb[width * height], width, height)
        {
        }

        private BitmapArgb(ColorArgb[] argb, int width, int height)
        {
            if (argb.Length != width * height)
                throw new ArgumentException("Bitmap array must have width * height pixels");

            this.argb = argb;
            Width = width;
            Height = height;
        }

        public ColorArgb this[int x, int y]
        {
            get => argb[x + y * Width];
            set => argb[x + y * Width] = value;
        }

        public static BitmapArgb From(Bitmap bitmap)
        {
            var argb = new ColorArgb[bitmap.Width * bitmap.Height];

            copyBytes(bitmap, argb, CopyMode.BitmapToArgb);

            return new BitmapArgb(argb, bitmap.Width, bitmap.Height);
        }

        public Bitmap ToSystemBitmap()
        {
            var bitmap = new Bitmap(Width, Height);

            copyBytes(bitmap, argb, CopyMode.ArgbToBitmap);

            return bitmap;
        }

        enum CopyMode
        {
            BitmapToArgb,
            ArgbToBitmap
        }

        private static void copyBytes(Bitmap bitmap, ColorArgb[] argb, CopyMode mode)
        {
            var data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                mode == CopyMode.BitmapToArgb ? ImageLockMode.ReadOnly : ImageLockMode.WriteOnly,
                SystemPixelFormat.Format32bppArgb
            );

            var bytes = bitmap.Width * bitmap.Height * 4;

            unsafe
            {
                fixed (void* pixels = argb)
                {
                    if (mode == CopyMode.BitmapToArgb)
                        Buffer.MemoryCopy(data.Scan0.ToPointer(), pixels, bytes, bytes);
                    else
                        Buffer.MemoryCopy(pixels, data.Scan0.ToPointer(), bytes, bytes);
                }
            }

            bitmap.UnlockBits(data);
        }
    }
}
