using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.Textures
{
    public sealed class ArrayTextureData
    {
        public static ArrayTextureData Empty { get; } = new ArrayTextureData(0, 0, ImmutableArray<TextureData>.Empty);

        public static ArrayTextureData From(IEnumerable<string> paths) => From(paths.Select(TextureData.From));

        public static ArrayTextureData From(IEnumerable<Stream> streams) => From(streams.Select(TextureData.From));

        public static ArrayTextureData From(IEnumerable<Bitmap> bitmaps) => From(bitmaps.Select(TextureData.From));

        public static ArrayTextureData From(
            IEnumerable<string> paths, IEnumerable<ITextureTransformation> transformations)
        {
            return From(paths.Select(stream => TextureData.From(stream, transformations)));
        }

        public static ArrayTextureData From(
            IEnumerable<Stream> streams, IEnumerable<ITextureTransformation> transformations)
        {
            return From(streams.Select(stream => TextureData.From(stream, transformations)));
        }

        public static ArrayTextureData From(
            IEnumerable<Bitmap> bitmaps, IEnumerable<ITextureTransformation> transformations)
        {
            return From(bitmaps.Select(bitmap => TextureData.From(bitmap, transformations)));
        }

        public static ArrayTextureData From(params TextureData[] textureData) =>
            From((IEnumerable<TextureData>) textureData);

        public static ArrayTextureData From(IEnumerable<TextureData> textureData)
        {
            var dataArray = ImmutableArray.CreateRange(textureData);
            if (dataArray.IsEmpty)
            {
                return Empty;
            }

            var width = dataArray[0].Width;
            var height = dataArray[0].Height;
            validateDimensions(dataArray, width, height);

            return new ArrayTextureData(width, height, dataArray);
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private static void validateDimensions(
            IEnumerable<TextureData> textureData, int expectedWidth, int expectedHeight)
        {
            if (textureData.Any(layer => layer.Width != expectedWidth || layer.Height != expectedHeight))
            {
                throw new ArgumentException("Texture does not have expected dimensions.");
            }
        }

        private readonly ImmutableArray<TextureData> textureData;

        public int Width { get; }

        public int Height { get; }

        public int LayerCount { get; }

        private ArrayTextureData(int width, int height, ImmutableArray<TextureData> textureData)
        {
            // It is the factory method's responsibility to pass in correct data.
            Width = width;
            Height = height;
            LayerCount = textureData.Length;
            this.textureData = textureData;
        }

        public ArrayTexture ToTexture(Action<ArrayTexture.Target> configure)
        {
            var texture = ToTexture();
            using var target = texture.Bind();
            configure(target);
            return texture;
        }

        public ArrayTexture ToTexture()
        {
            var texture = new ArrayTexture();
            PopulateTexture(texture);
            return texture;
        }

        public void PopulateTexture(ArrayTexture texture)
        {
            using var target = texture.Bind();

            target.Resize(Width, Height, LayerCount, PixelInternalFormat.Rgba);
            for (var i = 0; i < LayerCount; i++)
            {
                textureData[i].FillArrayTextureLayer(texture, i);
            }
        }
    }
}
