using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Textures;

public interface ITextureData
{
    int Width { get; }
    int Height { get; }
    void Upload(IUploadContext context);

    public interface IUploadContext
    {
        public void UploadFromPointer(IntPtr ptr, PixelFormat pixelFormat);
    }
}
