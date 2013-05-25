using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace amulware.Graphics
{
    class Font
    {
        public Vector2 PixelSymbolOffset { get; private set; }
        public Vector2 PixelSymbolSize { get; private set; }
        public Vector2 SymbolSize { get; private set; }

        public bool MonoSpaced { get; private set; }

        private float[] letterWidths;

        internal Font(
            Vector2 pixelSymbolOffset,
            Vector2 pixelSymbolSize,
            Vector2 symbolSize,
            float[] letterWidths = null
            )
        {
            this.PixelSymbolOffset = pixelSymbolOffset;
            this.PixelSymbolSize = pixelSymbolSize;
            this.SymbolSize = symbolSize;
            if (letterWidths != null &&
                letterWidths.Length == 256)
                this.letterWidths = letterWidths;
            this.MonoSpaced = this.letterWidths == null;
        }

        public float LetterWidth(int c)
        {
            return this.letterWidths[c];
        }
}
}
