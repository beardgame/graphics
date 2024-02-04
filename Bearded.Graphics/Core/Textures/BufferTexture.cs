using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures;

public sealed class BufferTexture : IBindableTexture<BufferTexture.Target>, IDisposable
{
    public int Handle { get; }

    public static BufferTexture ForBuffer<T>(Buffer<T> buffer, SizedInternalFormat format)
        where T : struct
    {
        var texture = new BufferTexture();

        using var target = texture.Bind();
        target.AttachBuffer(buffer, format);

        return texture;
    }

    public BufferTexture()
    {
        Handle = GL.GenTexture();
    }

    public Target Bind()
    {
        return new Target(this);
    }

    public readonly struct Target : IDisposable
    {
        internal Target(BufferTexture texture)
        {
            GL.BindTexture(TextureTarget.TextureBuffer, texture.Handle);
        }

        public void AttachBuffer<T>(Buffer<T> buffer, SizedInternalFormat internalFormat)
            where T : struct
        {
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, internalFormat, buffer.Handle);
        }

        public void DetachBuffer()
        {
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.R8, 0);
        }

        public void Dispose()
        {
            GL.BindTexture(TextureTarget.TextureBuffer, 0);
        }
    }

    public void Dispose()
    {
        GL.DeleteTexture(Handle);
    }
}
