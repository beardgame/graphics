using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Bearded.Graphics.Text;
using Bearded.Graphics.Textures;
using Bearded.Utilities.Algorithms;
using OpenTK.Mathematics;
using Font = Bearded.Graphics.Text.Font;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;
using SystemFont = System.Drawing.Font;
using SystemGraphics = System.Drawing.Graphics;
using SystemColor = System.Drawing.Color;

namespace Bearded.Graphics.System.Drawing;

public static class FontFactory
{
    public static (ITextureData, Font) From(SystemFont font, int paddingPixels = 0)
    {
        var printableAscii = Enumerable.Range(' ', '~' - ' ').Select(i => (char)i);
        return From(font, printableAscii, paddingPixels);
    }

    public static (ITextureData, Font) From(
        SystemFont font, IEnumerable<char> supportedCharacters, int paddingPixels = 0)
    {
        using var builder = new Builder(font, supportedCharacters, paddingPixels);

        return builder.Build();
    }

    private sealed class Builder : IDisposable
    {
        private const char fallbackGlyph = '�';

        private readonly SystemFont font;
        private readonly IEnumerable<char> supportedCharacters;
        private readonly int pixelPadding;

        private readonly Dictionary<char, MutableCharacterInfo> characterInfos;

        private readonly int[] measureBitmapData;
        private GCHandle measureBitmapDataHandle;
        private readonly Bitmap measureBitmap;

        public Builder(SystemFont font, IEnumerable<char> supportedCharacters, int pixelPadding)
        {
            this.font = font;
            this.supportedCharacters = supportedCharacters;
            this.pixelPadding = pixelPadding;

            characterInfos = new Dictionary<char, MutableCharacterInfo>();

            var dimension = font.Height * 3;
            measureBitmapData = new int[dimension * dimension];
            measureBitmapDataHandle = GCHandle.Alloc(measureBitmapData, GCHandleType.Pinned);
            measureBitmap = new Bitmap(
                dimension, dimension, dimension * 4,
                PixelFormat.Format32bppArgb, measureBitmapDataHandle.AddrOfPinnedObject());
        }

        public (ITextureData, Font) Build()
        {
            calculateAllCharacterSizes();
            var (packedWidth, packedHeight) = positionCharacters();

            return (buildTextureData(packedWidth, packedHeight), buildFont(packedWidth, packedHeight));
        }

        private void calculateAllCharacterSizes()
        {
            var stringFormat = StringFormat.GenericTypographic;
            stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            using var g = SystemGraphics.FromImage(measureBitmap);
            applyGraphicsSettings(g);
            var brush = new SolidBrush(SystemColor.White);
            var drawPos = new PointF(font.Height, font.Height);

            foreach (var c in supportedCharacters.Concat(new[] {fallbackGlyph}))
            {
                g.Clear(SystemColor.FromArgb(0, 0, 0, 0));
                g.DrawString(c.ToString(), font, brush, drawPos);

                var (xMin, xMax, yMin, yMax) = findBoundingBoxForCurrentBitmap();

                var characterInfo = new MutableCharacterInfo
                {
                    Size = new Vector2i(xMax - xMin + 1, yMax - yMin + 1),
                    Offset = new Vector2i(xMin - font.Height, yMin - font.Height),
                    SpacingWidth = g.MeasureString(c.ToString(), font, 0, stringFormat).Width,
                };
                characterInfos.Add(c, characterInfo);
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private (int XMin, int XMax, int YMin, int YMax) findBoundingBoxForCurrentBitmap()
        {
            var yRange = Enumerable.Range(0, measureBitmap.Height);

            var xMin = 0;
            while (xMin < measureBitmap.Width && yRange.All(y => getPixelArgb(xMin, y) == 0))
            {
                xMin++;
            }
            var xMax = measureBitmap.Width - 1;
            while (xMax >= xMin && yRange.All(y => getPixelArgb(xMax, y) == 0))
            {
                xMax--;
            }

            if (xMin == xMax && yRange.All(y => getPixelArgb(xMax, y) == 0))
            {
                return (0, 0, 0, 0);
            }

            var xRange = Enumerable.Range(xMin, xMax - xMin + 1);

            var yMin = 0;
            while (yMin < measureBitmap.Height && xRange.All(x => getPixelArgb(x, yMin) == 0))
            {
                yMin++;
            }
            var yMax = measureBitmap.Height - 1;
            while (yMax >= yMin && xRange.All(x => getPixelArgb(x, yMax) == 0))
            {
                yMax--;
            }

            return (xMin, xMax, yMin, yMax);
        }

        private int getPixelArgb(int x, int y)
        {
            var index = x + y * measureBitmap.Width;
            return measureBitmapData[index];
        }

        private (int PackedWidth, int PackedHeight) positionCharacters()
        {
            var result = BinPacking.Pack(
                characterInfos.Select(entry =>
                    new BinPacking.Rectangle<char>(
                        entry.Key, entry.Value.Size.X + 2 * pixelPadding, entry.Value.Size.Y + 2 * pixelPadding)));

            foreach (var rect in result.Rectangles)
            {
                characterInfos[rect.Value].TopLeft = new Vector2i(rect.X + pixelPadding, rect.Y + pixelPadding);
            }

            return (result.Width, result.Height);
        }

        private ITextureData buildTextureData(int width, int height)
        {
            var textureBitmap = new Bitmap(width, height);
            using var g = SystemGraphics.FromImage(textureBitmap);
            applyGraphicsSettings(g);

            // TODO: right now the background isn't transparent white as we want, since graphics doesn't support
            // clearing to transparent AND a color

            var brush = new SolidBrush(SystemColor.White);

            foreach (var (character, info) in characterInfos)
            {
                g.DrawString(
                    character.ToString(), font, brush,
                    info.TopLeft.X - info.Offset.X, info.TopLeft.Y - info.Offset.Y);
            }

            return textureDataFrom(textureBitmap);
        }

        private ITextureData textureDataFrom(Bitmap bitmap)
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

            return RawTextureData.From(array, bitmap.Width, bitmap.Height);

        }

        private Font buildFont(int textureWidth, int textureHeight)
        {
            return new Font(
                supportedCharacters.ToImmutableDictionary(
                    c => c,
                    c => toCharacterInfo(characterInfos[c], textureWidth, textureHeight)),
                toCharacterInfo(characterInfos[fallbackGlyph], textureWidth, textureHeight));
        }

        private CharacterInfo toCharacterInfo(MutableCharacterInfo info, int textureWidth, int textureHeight)
        {
            var fontHeightInverse = 1f / font.Height;
            var pixelSize = new Vector2(1f / textureWidth, 1f / textureHeight);

            return new CharacterInfo(
                info.Size.ToVector2() * fontHeightInverse,
                info.Offset.ToVector2() * fontHeightInverse,
                info.SpacingWidth * fontHeightInverse,
                pixelSize * info.TopLeft.ToVector2(),
                pixelSize * (info.TopLeft + info.Size).ToVector2());
        }

        public void Dispose()
        {
            measureBitmap.Dispose();
            measureBitmapDataHandle.Free();
        }

        private static void applyGraphicsSettings(SystemGraphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        private sealed class MutableCharacterInfo
        {
            public Vector2i Size { get; set; }
            public Vector2i Offset { get; set; }

            public float SpacingWidth { get; set; }

            public Vector2i TopLeft { get; set; }
        }
    }
}
