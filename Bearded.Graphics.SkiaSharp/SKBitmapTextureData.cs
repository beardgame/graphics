using System.Runtime.InteropServices;
using Bearded.Graphics.Textures;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Bearded.Graphics.SkiaSharp;

public sealed class SKBitmapTextureData : ITextureData
{
    private static readonly SKImageInfo info = SKImageInfo.Empty.WithColorType(SKColorType.Bgra8888);

    private readonly SKBitmap image;

    public int Width { get; }

    public int Height { get; }

    public static ITextureData From(string path) => new SKBitmapTextureData(SKBitmap.Decode(path, info));

    public static ITextureData From(Stream stream) => new SKBitmapTextureData(SKBitmap.Decode(stream, info));

    public static ITextureData From(SKBitmap bitmap) => new SKBitmapTextureData(bitmap);

    public static ITextureData From(string path, IEnumerable<ITextureTransformation> transformations)
    {
        using var image = SKBitmap.Decode(path);
        return From(image, transformations);
    }

    public static ITextureData From(Stream stream, IEnumerable<ITextureTransformation> transformations)
    {
        using var image = SKBitmap.Decode(stream);
        return From(image, transformations);
    }

    public static ITextureData From(SKBitmap image, IEnumerable<ITextureTransformation> transformations)
    {
        var bgraImage = image.ColorType == SKColorType.Bgra8888 ? image : image.Copy(SKColorType.Bgra8888);

        var width = image.Width;
        var height = image.Height;
        var size = width * height * 4;
        var array = new byte[size];

        var ptr = image.GetPixels();

        Marshal.Copy(ptr, array, 0, size);
        if (image != bgraImage)
            bgraImage.Dispose();

        foreach (var t in transformations)
        {
            t.Transform(ref array, ref width, ref height);
            RawTextureData.ValidateExpectedLength(array, width, height);
        }

        return RawTextureData.From(array, width, height);
    }

    private SKBitmapTextureData(SKBitmap image)
    {
        if (image.ColorType != SKColorType.Bgra8888)
            throw new ArgumentException("Image must be in BGRA format");

        this.image = image;
        Width = image.Width;
        Height = image.Height;
    }

    public void Upload(ITextureData.IUploadContext context)
    {
        var ptr = image.GetPixels();
        context.UploadFromPointer(ptr, PixelFormat.Bgra);
    }
}
