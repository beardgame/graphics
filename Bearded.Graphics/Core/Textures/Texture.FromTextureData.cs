using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures;

public sealed partial class Texture
{
    public static Texture From(ITextureData data, Action<Target> configure)
    {
        var texture = From(data);
        using var target = texture.Bind();
        configure(target);
        return texture;
    }

    public static Texture From(ITextureData data)
    {
        var texture = new Texture();
        texture.PopulateFrom(data);
        return texture;
    }

    public void PopulateFrom(ITextureData data)
    {
        using var context = new UploadContext(this, data);
        data.Upload(context);
    }

    private sealed record UploadContext(Texture Texture, ITextureData Data)
        : ITextureData.IUploadContext, IDisposable
    {
        private bool disposed;

        public void UploadFromPointer(IntPtr ptr, PixelFormat pixelFormat)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UploadContext));

            using var target = Texture.Bind();
            target.UploadData(
                ptr, pixelFormat, PixelType.UnsignedByte, Data.Width, Data.Height, PixelInternalFormat.Rgba);
        }

        public void Dispose()
        {
            disposed = true;
        }
    }
}
