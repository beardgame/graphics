using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures;

public sealed class RawTextureData : ITextureData
{
    private readonly byte[] data;

    public int Width { get; }
    public int Height { get; }

    public static ITextureData From(byte[] data, int width, int height)
    {
        ValidateExpectedLength(data, width, height);
        return new RawTextureData(data, width, height);
    }

    private RawTextureData(byte[] data, int width, int height)
    {
        this.data = data;
        Width = width;
        Height = height;
    }

    public void Upload(ITextureData.IUploadContext context)
    {
        var pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
        var ptr = pinnedArray.AddrOfPinnedObject();

        context.UploadFromPointer(ptr, PixelFormat.Bgra);

        pinnedArray.Free();
    }

    public static void ValidateExpectedLength(byte[] data, int width, int height)
    {
        var expectedLength = width * height * 4;
        if (data.Length != expectedLength)
        {
            throw new ArgumentException(
                "Array length does not equal width * height * 4. " +
                "Data should be in argb format, with four bytes per pixel."
            );
        }
    }
}
