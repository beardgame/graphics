using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace amulware.Graphics
{
    public sealed class ArrayTexture
    {
        public int Handle { get; }
        
        public int Width { get; }
        
        public int Height { get; }
        
        public int LayerCount { get; }

        public ArrayTexture(IList<Bitmap> layers, bool preMultiplyAlpha = false)
        {
            Width = layers[0].Width;
            Height = layers[0].Height;
            LayerCount = layers.Count;
            
            if (layers.Any(b => b.Width != Width || b.Height != Height))
                throw new ArgumentException("All layers must have the same dimensions.");
            
            GL.GenTextures(1, out int handle);
            Handle = handle;
            
            Bind();

            allocateStorage(Width, Height, LayerCount);

            foreach (var (bitmap, layer) in layers.Select((b, i) => (b, i)))
            {
                copyFromBitmap(bitmap, layer, preMultiplyAlpha);
            }
            
            generateMipmap();
            setDefaultParameters();
            
            Unbind();
        }

        private void copyFromBitmap(Bitmap bitmap, int layer, bool preMultiplyAlpha)
        {
            Texture.CopyDataFromBitmap(
                bitmap, preMultiplyAlpha,
                pointer => copyFromPointer(pointer, layer),
                array => copyFromArray(array, layer)
                );
        }

        private void copyFromArray(byte[] array, int layer)
        {
            GL.TexSubImage3D(
                TextureTarget.Texture2DArray, 0,
                0, 0, layer, Width, Height, 1,
                PixelFormat.Bgra, PixelType.UnsignedByte,
                array
            );
        }
        
        private void copyFromPointer(IntPtr pointer, int layer)
        {
            GL.TexSubImage3D(
                TextureTarget.Texture2DArray, 0,
                0, 0, layer, Width, Height, 1,
                PixelFormat.Bgra, PixelType.UnsignedByte,
                pointer
            );
        }
        
        private void allocateStorage(int width, int height, int layerCount)
        {
            GL.TexImage3D(
                TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba,
                Width, Height, layerCount,
                0, PixelFormat.Bgra, PixelType.UnsignedByte,
                IntPtr.Zero
                );
        }

        private static void generateMipmap()
        {
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }
        
        private static void setDefaultParameters()
        {
            setParameters(
                TextureMinFilter.LinearMipmapLinear,
                TextureMagFilter.Linear,
                TextureWrapMode.Repeat,
                TextureWrapMode.Repeat
            );
        }

        private static void setParameters(
            TextureMinFilter minFilter,
            TextureMagFilter magFilter,
            TextureWrapMode wrapS,
            TextureWrapMode wrapT)
        {
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int) minFilter);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int) magFilter);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int) wrapS);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int) wrapT);
        }
        
        public void Bind(TextureTarget target = TextureTarget.Texture2DArray)
        {
            GL.BindTexture(target, Handle);
        }

        public static void Unbind(TextureTarget target = TextureTarget.Texture2DArray)
        {
            GL.BindTexture(target, 0);
        }
        
        public static implicit operator int(ArrayTexture texture) => texture?.Handle ?? 0;

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }
    }
}
