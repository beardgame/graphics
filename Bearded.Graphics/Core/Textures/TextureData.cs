using System;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Bearded.Graphics.Textures;

public interface ITextureData
{
    int Width { get; }
    int Height { get; }
    Texture ToTexture(Action<Texture.Target> configure);
    Texture ToTexture();
    void PopulateTexture(Texture texture);
    void FillArrayTextureLayer(ArrayTexture texture, int layer);
}

public abstract class TextureData : ITextureData
{
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
        Upload(new SingleTextureUploadContext(texture, this));
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

        Upload(new ArrayTextureUploadContext(texture, layer));
    }

    protected abstract void Upload(ITextureUploadContext context);

    protected interface ITextureUploadContext
    {
        public void UploadFromPointer(IntPtr ptr, PixelFormat pixelFormat);
    }

    private sealed record SingleTextureUploadContext(Texture Texture, TextureData Data) : ITextureUploadContext
    {
        public void UploadFromPointer(IntPtr ptr, PixelFormat pixelFormat)
        {
            using var target = Texture.Bind();
            target.UploadData(
                ptr, pixelFormat, PixelType.UnsignedByte, Data.Width, Data.Height, PixelInternalFormat.Rgba);
        }
    }

    private sealed record ArrayTextureUploadContext(ArrayTexture Texture, int Layer) : ITextureUploadContext
    {
        public void UploadFromPointer(IntPtr ptr, PixelFormat pixelFormat)
        {
            using var target = Texture.Bind();
            target.UploadLayer(ptr, pixelFormat, PixelType.UnsignedByte, Layer);
        }
    }
}
