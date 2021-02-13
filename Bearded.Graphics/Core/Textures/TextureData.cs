using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace amulware.Graphics.Textures
{
    public abstract class TextureData
    {
        public static TextureData From(byte[] data, int width, int height)
        {
            validateExpectedLength(data, width, height);
            return new RawTextureData(data, width, height);
        }

        public static TextureData From(string path) => From(new Bitmap(path));

        public static TextureData From(Stream stream) => From(new Bitmap(stream));

        public static TextureData From(Bitmap bitmap) => new BitmapTextureData(bitmap);

        public static TextureData From(string path, IEnumerable<ITextureTransformation> transformations)
        {
            using var bitmap = new Bitmap(path);
            return From(bitmap, transformations);
        }

        public static TextureData From(Stream stream, IEnumerable<ITextureTransformation> transformations)
        {
            using var bitmap = new Bitmap(stream);
            return From(bitmap, transformations);
        }

        public static TextureData From(Bitmap bitmap, IEnumerable<ITextureTransformation> transformations)
        {
            var data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                SystemPixelFormat.Format32bppArgb
            );
            var size = data.Width * data.Height * 4;
            var array = new byte[size];
            Marshal.Copy(data.Scan0, array, 0, size);

            bitmap.UnlockBits(data);

            var width = bitmap.Width;
            var height = bitmap.Height;

            foreach (var t in transformations)
            {
                t.Transform(ref array, ref width, ref height);
                validateExpectedLength(array, width, height);
            }

            return new RawTextureData(array, width, height);
        }

        private static void validateExpectedLength(byte[] data, int width, int height)
        {
            var expectedLength = width * height * 4;
            if (data.Length != expectedLength)
            {
                throw new ArgumentException(
                    "Array length does not equal width * height * 4. " +
                    "Data should be in argb format, with four bytes per pixel."
                );
            }
        }

        public abstract int Width { get; }

        public abstract int Height { get; }

        public Texture ToTexture(Action<Texture.Target> configure)
        {
            var texture = ToTexture();
            using var target = texture.Bind();
            configure(target);
            return texture;
        }

        public Texture ToTexture()
        {
            var texture = new Texture();
            PopulateTexture(texture);
            return texture;
        }

        public void PopulateTexture(Texture texture)
        {
            using var target = texture.Bind();
            Visit(ptr =>
            {
                // ReSharper disable once AccessToDisposedClosure
                // we trust the implementation to behave
                target.UploadData(
                    ptr, PixelFormat.Bgra, PixelType.UnsignedByte, Width, Height, PixelInternalFormat.Rgba);
            });
        }

        public void FillArrayTextureLayer(ArrayTexture texture, int layer)
        {
            if (texture.Width != Width || texture.Height != Height)
            {
                throw new ArgumentException("Texture data does not have same dimensions as array texture.");
            }
            if (layer < 0 || layer >= texture.LayerCount)
            {
                throw new ArgumentOutOfRangeException(nameof(layer));
            }

            using var target = texture.Bind();
            Visit(ptr =>
            {
                // ReSharper disable once AccessToDisposedClosure
                // we trust the implementation to behave
                target.UploadLayer(ptr, PixelFormat.Bgra, PixelType.UnsignedByte, layer);
            });
        }

        protected abstract void Visit(Action<IntPtr> action);

        private sealed class RawTextureData : TextureData
        {
            private readonly byte[] data;

            public override int Width { get; }
            public override int Height { get; }

            public RawTextureData(byte[] data, int width, int height)
            {
                this.data = data;
                Width = width;
                Height = height;
            }

            protected override void Visit(Action<IntPtr> action)
            {
                var pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
                var ptr = pinnedArray.AddrOfPinnedObject();

                action(ptr);

                pinnedArray.Free();
            }
        }

        private sealed class BitmapTextureData : TextureData
        {
            private readonly Bitmap bitmap;

            public override int Width { get; }
            public override int Height { get; }

            public BitmapTextureData(Bitmap bitmap)
            {
                this.bitmap = bitmap;
                Width = bitmap.Width;
                Height = bitmap.Height;
            }

            protected override void Visit(Action<IntPtr> action)
            {
                var lockedData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    SystemPixelFormat.Format32bppArgb);
                action(lockedData.Scan0);
                bitmap.UnlockBits(lockedData);
            }
        }
    }
}
