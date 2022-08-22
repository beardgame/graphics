using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bearded.Graphics.Textures;

public sealed class ArrayTextureData
{
    public static ArrayTextureData Empty { get; } = new(0, 0, ImmutableArray<ITextureData>.Empty);

    public static ArrayTextureData From(params ITextureData[] textureData) =>
        From((IEnumerable<ITextureData>) textureData);

    public static ArrayTextureData From(IEnumerable<ITextureData> textureData)
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
        IEnumerable<ITextureData> textureData, int expectedWidth, int expectedHeight)
    {
        if (textureData.Any(layer => layer.Width != expectedWidth || layer.Height != expectedHeight))
        {
            throw new ArgumentException("Texture does not have expected dimensions.");
        }
    }

    private readonly ImmutableArray<ITextureData> textureData;

    public int Width { get; }

    public int Height { get; }

    public int LayerCount { get; }

    public ITextureData this[int layer] => textureData[layer];

    private ArrayTextureData(int width, int height, ImmutableArray<ITextureData> textureData)
    {
        // It is the factory method's responsibility to pass in correct data.
        Width = width;
        Height = height;
        LayerCount = textureData.Length;
        this.textureData = textureData;
    }
}
