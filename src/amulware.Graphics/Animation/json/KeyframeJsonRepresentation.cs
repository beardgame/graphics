using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    internal class KeyframeJsonRepresentation
    {
        public string Name { get; set; }
        public List<KeyframeDataJsonRepresentation> Data { get; set; }
    }
}
