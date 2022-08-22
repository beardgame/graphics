using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Bearded.Graphics.Textures;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Bearded.Graphics.System.Drawing;

public sealed class BitmapTextureData : ITextureData
{
    private readonly Bitmap bitmap;

    public int Width { get; }
    public int Height { get; }

    public static ITextureData From(string path) => From(new Bitmap(path));

    public static ITextureData From(Stream stream) => From(new Bitmap(stream));

    public static ITextureData From(Bitmap bitmap) => new BitmapTextureData(bitmap);

    public static ITextureData From(string path, IEnumerable<ITextureTransformation> transformations)
    {
        using var bitmap = new Bitmap(path);
        return From(bitmap, transformations);
    }

    public static ITextureData From(Stream stream, IEnumerable<ITextureTransformation> transformations)
    {
        using var bitmap = new Bitmap(stream);
        return From(bitmap, transformations);
    }

    public static ITextureData From(Bitmap bitmap, IEnumerable<ITextureTransformation> transformations)
    {
        var data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly,
            SystemPixelFormat.Format32bppArgb
        );
        var size = data.Width * data.Height * 4;
        var array = new byte[size];
        Marshal.Copy(data.Scan0, array, 0, size);

        bitmap.UnlockBits(data);

        var width = bitmap.Width;
        var height = bitmap.Height;

        foreach (var t in transformations)
        {
            t.Transform(ref array, ref width, ref height);
            RawTextureData.ValidateExpectedLength(array, width, height);
        }

        return RawTextureData.From(array, width, height);
    }

    public BitmapTextureData(Bitmap bitmap)
    {
        this.bitmap = bitmap;
        Width = bitmap.Width;
        Height = bitmap.Height;
    }

    public void Upload(ITextureData.IUploadContext context)
    {
        var lockedData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly,
            SystemPixelFormat.Format32bppArgb);
        context.UploadFromPointer(lockedData.Scan0, PixelFormat.Bgra);
        bitmap.UnlockBits(lockedData);
    }
}
