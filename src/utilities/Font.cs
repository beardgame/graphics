using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

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
        public Vector2 PixelSymbolOffset { get; private set; }
        /// <summary>
        /// Syze of the symbol in the texture in uv coordinates.
        /// </summary>
        public Vector2 PixelSymbolSize { get; private set; }
        /// <summary>
        /// Default size of a symbol when drawing text with this font.
        /// </summary>
        public Vector2 SymbolSize { get; private set; }

        /// <summary>
        /// Whether this font is monospaced.
        /// </summary>
        public bool MonoSpaced { get; private set; }

        private float[] letterWidths;


        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> class.
        /// </summary>
        /// <param name="pixelSymbolOffset">The offset of the 0-char in the texture in uv coordinates.</param>
        /// <param name="pixelSymbolSize">Size of a symbol in the texture in uv coordinates.</param>
        /// <param name="symbolSize">Default size of a symbol when drawing text.</param>
        /// <param name="letterWidths">Relative widths of the symbols. Null for monospaced font.</param>
        internal Font(Vector2 pixelSymbolOffset, Vector2 pixelSymbolSize,
            Vector2 symbolSize, float[] letterWidths = null)
        {
            this.PixelSymbolOffset = pixelSymbolOffset;
            this.PixelSymbolSize = pixelSymbolSize;
            this.SymbolSize = symbolSize;
            if (letterWidths != null)
            {
                this.letterWidths = new float[256];
                int max = Math.Max(letterWidths.Length, 256);
                Array.Copy(letterWidths, this.letterWidths, max);
            }
            this.MonoSpaced = this.letterWidths == null;
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
    }
}
