using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures
{
    public sealed partial class ArrayTexture : IDisposable
    {
        private PixelInternalFormat pixelInternalFormat;

        public int Handle { get; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int LayerCount { get; private set; }

        public static ArrayTexture Empty(
            int width, int height, int layerCount, PixelInternalFormat pixelFormat = PixelInternalFormat.Rgba)
        {
            var arrayTexture = new ArrayTexture();

            using var target = arrayTexture.Bind();
            target.Resize(width, height, layerCount, pixelFormat);
            target.SetFilterMode(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear);
            target.SetWrapMode(TextureWrapMode.Repeat, TextureWrapMode.Repeat);

            return arrayTexture;
        }

        public ArrayTexture()
        {
            GL.GenTextures(1, out int handle);
            Handle = handle;
        }

        public Target Bind(TextureTarget target = TextureTarget.Texture2DArray)
        {
            return new Target(this, target);
        }

        public readonly struct Target : IDisposable
        {
            private readonly ArrayTexture arrayTexture;
            private readonly TextureTarget target;

            internal Target(ArrayTexture arrayTexture, TextureTarget target)
            {
                this.arrayTexture = arrayTexture;
                this.target = target;
                GL.BindTexture(target, arrayTexture.Handle);
            }

            public void GenerateMipmap()
            {
                GL.GenerateMipmap((GenerateMipmapTarget)target);
            }

            public void Dispose()
            {
                GL.BindTexture(target, 0);
            }

            public void Resize(int width, int height)
            {
                Resize(width, height, arrayTexture.LayerCount, arrayTexture.pixelInternalFormat);
            }

            public void Resize(int width, int height, int layerCount)
            {
                Resize(width, height, layerCount, arrayTexture.pixelInternalFormat);
            }

            public void Resize(int width, int height, int layerCount, PixelInternalFormat pixelFormat)
            {
                UploadData(IntPtr.Zero, PixelFormat.Rgba, PixelType.UnsignedByte, width, height, layerCount, pixelFormat);
            }

            public void UploadData(IntPtr ptr, PixelFormat pixelFormat, PixelType pixelType, int width, int height, int layerCount, PixelInternalFormat pixelInternalFormat)
            {
                GL.TexImage3D(
                    target, 0,
                    pixelInternalFormat, width, height, layerCount, 0,
                    pixelFormat, pixelType,
                    ptr);
                arrayTexture.Width = width;
                arrayTexture.Height = height;
                arrayTexture.LayerCount = layerCount;
                arrayTexture.pixelInternalFormat = pixelInternalFormat;
            }

            public void UploadLayer(IntPtr ptr, PixelFormat pixelFormat, PixelType pixelType, int layer)
            {
                GL.TexSubImage3D(
                    target, 0,
                    0, 0, layer,
                    arrayTexture.Width, arrayTexture.Height, 1,
                    pixelFormat, pixelType,
                    ptr);
            }

            public void SetFilterMode(TextureMinFilter minFilter, TextureMagFilter magFilter)
            {
                GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int) minFilter);
                GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int) magFilter);
            }

            public void SetWrapMode(TextureWrapMode wrapHorizontal, TextureWrapMode wrapVertical)
            {
                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int) wrapHorizontal);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int) wrapVertical);
            }
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }
    }
}
