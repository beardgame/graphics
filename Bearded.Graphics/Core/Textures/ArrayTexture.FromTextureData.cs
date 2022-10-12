using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures;

public sealed partial class ArrayTexture
{
    public static ArrayTexture From(ArrayTextureData data, Action<Target> configure)
    {
        var texture = From(data);
        using var target = texture.Bind();
        configure(target);
        return texture;
    }

    public static ArrayTexture From(ArrayTextureData data)
    {
        var texture = new ArrayTexture();
        texture.PopulateFrom(data);
        return texture;
    }

    public void PopulateFrom(ArrayTextureData data)
    {
        using var target = Bind();
        using var context = new UploadContext(target);

        target.Resize(data.Width, data.Height, data.LayerCount, PixelInternalFormat.Rgba);

        for (var i = 0; i < LayerCount; i++)
        {
            context.Layer = i;
            data[i].Upload(context);
        }
    }

    public void PopulateLayerFrom(ITextureData data, int layer)
    {
        if (data.Width != Width || data.Height != Height)
            throw new ArgumentException("Texture data does not have same dimensions as array texture.");
        if (layer < 0 || layer >= LayerCount)
            throw new ArgumentOutOfRangeException(nameof(layer));

        using var target = Bind();
        using var context = new UploadContext(target) { Layer = layer };
        data.Upload(context);
    }

    private sealed record UploadContext(Target Target)
        : ITextureData.IUploadContext, IDisposable
    {
        private bool disposed;

        public int Layer { get; set; }

        public void UploadFromPointer(IntPtr ptr, PixelFormat pixelFormat)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UploadContext));

            Target.UploadLayer(ptr, pixelFormat, PixelType.UnsignedByte, Layer);
        }

        public void Dispose()
        {
            disposed = true;
        }
    }
}
