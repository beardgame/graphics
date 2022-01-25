using System.Collections.Generic;
using System.Drawing;
using Bearded.Graphics.Content;
using OpenTK.Mathematics;

namespace Bearded.Graphics.TextureProcessor
{
    record struct NamedBitmap(Bitmap Bitmap, string Name);

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

        public IEnumerable<NamedBitmap> Process()
        {
            yield return new (source.ToSystemBitmap(c => new Color((uint)c.ToArgb())), "Input");
            yield return new (source.ToSystemBitmap(c => new Color((uint)c.ToArgb())), "Output");
        }
    }
}
