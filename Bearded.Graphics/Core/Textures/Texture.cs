using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures
{
    public sealed partial class Texture : IBindableTexture<Texture.Target>, IDisposable
    {
        private readonly TextureTarget target;
        private PixelInternalFormat pixelInternalFormat;
        private PixelFormat pixelFormat;
        private PixelType pixelType;

        public int Handle { get; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public static Texture Empty(int width, int height,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType pixelType = PixelType.UnsignedByte,
            TextureTarget textureTarget = TextureTarget.Texture2D)
        {
            var texture = new Texture(textureTarget);

            using var target = texture.Bind();
            target.Resize(width, height, pixelInternalFormat, pixelFormat, pixelType);
            target.SetFilterMode(TextureMinFilter.Linear, TextureMagFilter.Linear);
            target.SetWrapMode(TextureWrapMode.Repeat, TextureWrapMode.Repeat);

            return texture;
        }

        public Texture(TextureTarget target = TextureTarget.Texture2D)
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
            private readonly Texture texture;
            private TextureTarget target => texture.target;

            internal Target(Texture texture)
            {
                this.texture = texture;
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
