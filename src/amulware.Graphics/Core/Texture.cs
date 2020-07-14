using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class Texture : IDisposable
    {
        public int Handle { get; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public static Texture Empty(int width, int height, PixelInternalFormat pixelFormat = PixelInternalFormat.Rgba)
        {
            var texture = new Texture();

            using var target = texture.Bind();
            target.Resize(width, height, pixelFormat);
            target.SetParameters(
                TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear,
                TextureWrapMode.Repeat, TextureWrapMode.Repeat
            );

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

            public Target(Texture texture, TextureTarget target)
            {
                this.texture = texture;
                this.target = target;
                GL.BindTexture(target, texture.Handle);
            }

            public void GenerateMipmap()
            {
                GL.GenerateMipmap((GenerateMipmapTarget)target);
            }

            public void Resize(int width, int height, PixelInternalFormat pixelFormat)
            {
                UploadData(IntPtr.Zero, width, height, pixelFormat);
            }

            public void UploadData(IntPtr ptr, int width, int height, PixelInternalFormat pixelFormat)
            {
                GL.TexImage2D(
                    target, 0,
                    pixelFormat, width, height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte,
                    ptr);
                texture.Width = width;
                texture.Height = height;
            }

            public void SetParameters(
                TextureMinFilter minFilter,
                TextureMagFilter magFilter,
                TextureWrapMode wrapS,
                TextureWrapMode wrapT)
            {
                GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int) minFilter);
                GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int) magFilter);
                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int) wrapS);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int) wrapT);
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
