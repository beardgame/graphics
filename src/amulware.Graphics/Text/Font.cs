using System;
using System.IO;
using amulware.Graphics.Serialization.JsonNet;
using Newtonsoft.Json;
using OpenToolkit.Mathematics;

namespace amulware.Graphics
{
    /// <summary>
    /// Immutable class representing a font.
    /// </summary>
    public sealed class Font
    {
        /// <summary>
        /// The offset of the 0-char in the texture in uv coordinates.
        /// </summary>
        public Vector2 UVSymbolOffset { get; private set; }
        /// <summary>
        /// Size of the symbol in the texture in uv coordinates.
        /// </summary>
        public Vector2 UVSymbolSize { get; private set; }
        /// <summary>
        /// Default size of a symbol when drawing text with this font.
        /// </summary>
        public Vector2 SymbolSize { get; private set; }

        /// <summary>
        /// Whether this font is monospaced.
        /// </summary>
        public bool Monospaced { get; private set; }

        private float[] letterWidths;


        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> class.
        /// </summary>
        /// <param name="builder">The <see cref="Builder"/> to create the font from.</param>
        private Font(Builder builder)
        {
            this.UVSymbolOffset = divideComponents(builder.UVSymbolOffset, builder.UVSize);
            this.UVSymbolSize = divideComponents(builder.UVSymbolSize, builder.UVSize);
            //TODO: figure out what exactly symbol size means...
            // is it just the same as the fontgeometry.sizecoefficient?
            // is that needed?
            // how does it behave differently when monospaced or proportional?
            this.SymbolSize = builder.SymbolSize;
            if (builder.LetterWidths != null)
            {
                this.letterWidths = new float[256];
                int max = Math.Max(builder.LetterWidths.Length, 256);
                Array.Copy(builder.LetterWidths, this.letterWidths, max);
                for (int i = 0; i < 256; i++)
                    this.letterWidths[i] /= builder.UVSize.X;
            }
            this.Monospaced = this.letterWidths == null;
        }

        private static Vector2 divideComponents(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static Font FromJsonFile(string filename)
        {
            return JsonConvert.DeserializeObject<Font>(
                File.ReadAllText(filename),
                new JsonSerializerSettings().ConfigureForGraphics()
                );
        }

        /// <summary>
        /// Relative width of a symbol. Throws an exception if the font is monospaced.
        /// </summary>
        /// <param name="c">ASCII code of symbol. Range: [0, 255]</param>
        /// <returns>Relative width</returns>
        public float LetterWidth(int c)
        {
            return this.letterWidths[c];
        }

        /// <summary>
        /// Font builder, used to create <see cref="Font"/>s.
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// Represents the dimension of the texture that will be sampled from.
            /// When building a font, the uv-coordinates are divided by this scalar.
            /// </summary>
            public Vector2 UVSize { get; set; }

            /// <summary>
            /// The offset of the 0-char in the texture in uv coordinates.
            /// </summary>
            public Vector2 UVSymbolOffset { get; set; }
            /// <summary>
            /// Syze of the symbol in the texture in uv coordinates.
            /// </summary>
            public Vector2 UVSymbolSize { get; set; }
            /// <summary>
            /// Default size of a symbol when drawing text with built fonts.
            /// </summary>
            public Vector2 SymbolSize { get; set; }

            /// <summary>
            /// Default size of a symbol when drawing text with built fonts.
            /// </summary>
            public float[] LetterWidths { get; set; }

            /// <summary>
            /// Initialises a new font builder with default settings.
            /// </summary>
            public Builder()
            {
                this.UVSymbolSize = new Vector2(1f / 16f, 1f / 16f);
                this.UVSymbolOffset = Vector2.Zero;
                this.SymbolSize = Vector2.One;
                this.UVSize = Vector2.One;
                this.LetterWidths = null;
            }

            /// <summary>
            /// Builds a <see cref="Font"/> from the current settings.
            /// </summary>
            /// <returns>A <see cref="Font"/>.</returns>
            public Font Build()
            {
                return new Font(this);
            }
        }
    }
}
