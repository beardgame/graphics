using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace amulware.Graphics
{
    public sealed class Texture : IDisposable
    {
        public int Handle { get; private set; }
        
        public int Height { get; private set; }
        
        public int Width { get; private set; }
        
        public Texture(Stream bitmapFileStream, bool preMultiplyAlpha = false)
            : this(new Bitmap(bitmapFileStream), preMultiplyAlpha, true)
        {
        }

        public Texture(string imageFilePath, bool preMultiplyAlpha = false)
            : this(new Bitmap(imageFilePath), preMultiplyAlpha, true)
        {
        }

        private Texture(Bitmap bitmap, bool preMultiplyAlpha = false, bool disposeBitmap = false)
            : this()
        {
            Width = bitmap.Width;
            Height = bitmap.Height;

            Bind();

            copyFromBitmap(bitmap, preMultiplyAlpha);

            if (disposeBitmap)
                bitmap.Dispose();

            generateMipmap();

            setDefaultParameters();

            Unbind();
        }

        public Texture(byte[] data, int width, int height)
            : this()
        {
            var expectedLength = width * height * 4;
            if (data.Length != expectedLength)
            {
                throw new ArgumentException(
                    "Array length does not equal width * height * 4." +
                    "Data should be in Argb format, with four bytes per pixel."
                );
            }

            Width = width;
            Height = height;

            Bind();

            copyFromArray(data);
            generateMipmap();
            setDefaultParameters();

            Unbind();
        }

        public Texture(int width, int height, PixelInternalFormat pixelFormat = PixelInternalFormat.Rgba)
            : this()
        {
            Bind();

            resize(width, height, PixelInternalFormat.Rgba);
            setDefaultParameters();

            Unbind();
        }

        public Texture()
        {
            GL.GenTextures(1, out int handle);
            Handle = handle;
        }

        private void copyFromBitmap(Bitmap bitmap, bool preMultiplyAlpha)
        {
            var data = bitmap.LockBits(
                new Rectangle(0, 0, Width, Height),
                ImageLockMode.ReadOnly,
                SystemPixelFormat.Format32bppArgb
            );

            if (preMultiplyAlpha)
            {
                var size = data.Width * data.Height * 4;
                var array = new byte[size];
                Marshal.Copy(data.Scan0, array, 0, size);

                bitmap.UnlockBits(data);

                PreMultipleArgbArray(array);

                copyFromArray(array);
            }
            else
            {
                copyFromPointer(data.Scan0);

                bitmap.UnlockBits(data);
            }
        }

        public static void PreMultipleArgbArray(byte[] data)
        {
            var size = data.Length;
            for (var i = 0; i < size; i += 4)
            {
                var alpha = data[i + 3] / 255f;
                data[i] = (byte)(data[i] * alpha);
                data[i + 1] = (byte)(data[i + 1] * alpha);
                data[i + 2] = (byte)(data[i + 2] * alpha);
            }
        }

        private void copyFromArray(byte[] array)
        {
            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, Width, Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte,
                array
            );
        }

        private void copyFromPointer(IntPtr pointer)
        {
            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, Width, Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte,
                pointer
            );
        }

        public void Resize(int width, int height, PixelInternalFormat pixelFormat = PixelInternalFormat.Rgba)
        {
            Bind();

            resize(width, height, pixelFormat);

            Unbind();
        }

        private void resize(int width, int height, PixelInternalFormat pixelFormat)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, pixelFormat, width, height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            Width = width;
            Height = height;
        }

        public void GenerateMipmap()
        {
            Bind();

            generateMipmap();

            Unbind();
        }

        private static void generateMipmap()
        {
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void SetParameters(TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT)
        {
            Bind();

            setParameters(minFilter, magFilter, wrapS, wrapT);

            Unbind();
        }

        private static void setDefaultParameters()
        {
            setParameters(
                TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear,
                TextureWrapMode.Repeat, TextureWrapMode.Repeat
            );
        }

        private static void setParameters(
            TextureMinFilter minFilter, TextureMagFilter magFilter,
            TextureWrapMode wrapS, TextureWrapMode wrapT)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) magFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) wrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) wrapT);
        }
        
        public void Bind(TextureTarget target = TextureTarget.Texture2D)
        {
            GL.BindTexture(target, Handle);
        }

        public static void Unbind(TextureTarget target = TextureTarget.Texture2D)
        {
            GL.BindTexture(target, 0);
        }
        
        public UVRectangle GrabUV(Vector2 position, Vector2 size)
            => GrabUV(position.X, position.Y, size.X, size.Y);

        public UVRectangle GrabUV(float x, float y, float w, float h)
        {
            return new UVRectangle(
                x / Width,
                (x + w) / Width,
                y / Height,
                (y + h) / Height
            );
        }

        public static implicit operator int(Texture texture) => texture?.Handle ?? 0;

        public void Dispose()
        {
            if (Handle == 0)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteTexture(this);
            Handle = 0;
        }
    }
}
