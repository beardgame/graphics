using Bearded.Graphics.Textures;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Bearded.Graphics.ImageSharp;

public sealed class ImageTextureData : ITextureData
{
    private static readonly Configuration configuration;

    static ImageTextureData()
    {
        configuration = Configuration.Default.Clone();
        configuration.PreferContiguousImageBuffers = true;
    }

    private readonly Image<Bgra32> image;

    public int Width { get; }

    public int Height { get; }

    public static ITextureData From(string path) => new ImageTextureData(Image.Load<Bgra32>(configuration, path));

    public static ITextureData From(Stream stream) => new ImageTextureData(Image.Load<Bgra32>(configuration, stream));

    public static ITextureData From(Image bitmap) => new ImageTextureData(bitmap.CloneAs<Bgra32>(configuration));

    public static ITextureData From(string path, IEnumerable<ITextureTransformation> transformations)
    {
        using var image = Image.Load<Bgra32>(configuration, path);
        return From(image, transformations);
    }

    public static ITextureData From(Stream stream, IEnumerable<ITextureTransformation> transformations)
    {
        using var image = Image.Load<Bgra32>(configuration, stream);
        return From(image, transformations);
    }

    public static ITextureData From(Image image, IEnumerable<ITextureTransformation> transformations)
    {
        var bgraImage = image as Image<Bgra32> ?? image.CloneAs<Bgra32>();

        var width = image.Width;
        var height = image.Height;
        var size = width * height * 4;
        var array = new byte[size];

        bgraImage.CopyPixelDataTo(array);
        if (image != bgraImage)
            bgraImage.Dispose();

        foreach (var t in transformations)
        {
            t.Transform(ref array, ref width, ref height);
            RawTextureData.ValidateExpectedLength(array, width, height);
        }

        return RawTextureData.From(array, width, height);
    }

    private ImageTextureData(Image<Bgra32> image)
    {
        if (!image.DangerousTryGetSinglePixelMemory(out _))
            throw new InvalidOperationException("Image is not a single-array image");

        this.image = image;
        Width = image.Width;
        Height = image.Height;
    }

    public void Upload(ITextureData.IUploadContext context)
    {
        unsafe
        {
            if (!image.DangerousTryGetSinglePixelMemory(out var memory))
                throw new InvalidOperationException("Image is not a single-array image");
            using var handle = memory.Pin();
            context.UploadFromPointer(new IntPtr(handle.Pointer), PixelFormat.Bgra);
        }
    }
}
