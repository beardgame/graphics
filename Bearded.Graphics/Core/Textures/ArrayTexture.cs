using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures
{
    public sealed partial class ArrayTexture : IBindableTexture<ArrayTexture.Target>, IDisposable
    {
        private readonly TextureTarget target;
        private PixelInternalFormat pixelInternalFormat;

        public int Handle { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LayerCount { get; private set; }

        public static ArrayTexture Empty(
            int width, int height, int layerCount,
            PixelInternalFormat pixelFormat = PixelInternalFormat.Rgba,
            TextureTarget textureTarget = TextureTarget.Texture2DArray)
        {
            var arrayTexture = new ArrayTexture(textureTarget);

            using var target = arrayTexture.Bind();
            target.Resize(width, height, layerCount, pixelFormat);
            target.SetFilterMode(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear);
            target.SetWrapMode(TextureWrapMode.Repeat, TextureWrapMode.Repeat);

            return arrayTexture;
        }

        public ArrayTexture(TextureTarget target = TextureTarget.Texture2DArray)
        {
            this.target = target;
            Handle = GL.GenTexture();
        }

        public Target Bind()
        {
            return new Target(this);
        }

        public readonly struct Target : IDisposable
        {
            private readonly ArrayTexture arrayTexture;
            private TextureTarget target => arrayTexture.target;

            internal Target(ArrayTexture arrayTexture)
            {
                this.arrayTexture = arrayTexture;
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
