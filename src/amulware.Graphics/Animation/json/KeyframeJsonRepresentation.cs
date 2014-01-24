using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    internal class KeyframeJsonRepresentation<TKeyframeParameters>
    {
        public string Name { get; set; }
        public List<KeyframeDataJsonRepresentation<TKeyframeParameters>> Data { get; set; }
    }
}
