using System.Collections.Generic;
using System.Drawing;
using Bearded.Graphics.Content;
using OpenTK.Mathematics;

namespace Bearded.Graphics.TextureProcessor
{
    sealed class Processor
    {
        private readonly Bitmap<Color4> source;

        public static Processor From(Bitmap bitmap)
        {
            return new Processor(Bitmap<Color4>.From(bitmap, c => new Color4(c.R, c.G, c.B, c.A)));
        }

        private Processor(Bitmap<Color4> source)
        {
            this.source = source;
        }

        public IEnumerable<Bitmap> Process()
        {
            yield return source.ToSystemBitmap(c => new Color((uint)c.ToArgb()));
            yield return source.ToSystemBitmap(c => new Color((uint)c.ToArgb()));
        }
    }
}
