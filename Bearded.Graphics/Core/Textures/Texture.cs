using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures
{
    public sealed partial class Texture : IDisposable
    {
        private PixelInternalFormat pixelInternalFormat;
        private PixelFormat pixelFormat;
        private PixelType pixelType;

        public int Handle { get; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public static Texture Empty(int width, int height,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType pixelType = PixelType.UnsignedByte)
        {
            var texture = new Texture();

            using var target = texture.Bind();
            target.Resize(width, height, pixelInternalFormat, pixelFormat, pixelType);
            target.SetFilterMode(TextureMinFilter.Linear, TextureMagFilter.Linear);
            target.SetWrapMode(TextureWrapMode.Repeat, TextureWrapMode.Repeat);

            return texture;
        }

        public Texture()
        {
            GL.GenTextures(1, out int handle);
            Handle = handle;
        }

        public Target Bind(TextureTarget target = TextureTarget.Texture2D)
        {
            return new Target(this, target);
        }

        public readonly struct Target : IDisposable
        {
            private readonly Texture texture;
            private readonly TextureTarget target;

            internal Target(Texture texture, TextureTarget target)
            {
                this.texture = texture;
                this.target = target;
                GL.BindTexture(target, texture.Handle);
            }

            public void GenerateMipmap()
            {
                GL.GenerateMipmap((GenerateMipmapTarget)target);
            }

            public void Resize(int width, int height)
            {
                Resize(width, height, texture.pixelInternalFormat, texture.pixelFormat, texture.pixelType);
            }

            public void Resize(int width, int height,
                PixelInternalFormat pixelInternalFormat,
                PixelFormat pixelFormat, PixelType pixelType)
            {
                UploadData(IntPtr.Zero, pixelFormat, pixelType, width, height, pixelInternalFormat);
            }

            public void UploadData(IntPtr ptr, PixelFormat pixelFormat, PixelType pixelType,
                int width, int height, PixelInternalFormat pixelInternalFormat)
            {
                GL.TexImage2D(
                    target, 0,
                    pixelInternalFormat, width, height, 0,
                    pixelFormat, pixelType,
                    ptr);
                texture.Width = width;
                texture.Height = height;
                texture.pixelInternalFormat = pixelInternalFormat;
                texture.pixelFormat = pixelFormat;
                texture.pixelType = pixelType;
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

            public void Dispose()
            {
                GL.BindTexture(target, 0);
            }
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }
    }
}
