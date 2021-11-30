using System.Collections.Generic;
using System.Drawing;
using Bearded.Graphics.Content;

namespace Bearded.Graphics.TextureProcessor
{
    sealed class Processor
    {
        private readonly BitmapArgb source;

        public static Processor From(Bitmap bitmap)
        {
            return new Processor(BitmapArgb.From(bitmap));
        }

        private Processor(BitmapArgb source)
        {
            this.source = source;
        }

        public IEnumerable<BitmapArgb> Process()
        {
            yield return source;
            yield return source;
        }
    }
}
