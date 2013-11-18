using OpenTK;

namespace amulware.Graphics.Animation
{
    internal class KeyframeDataJsonRepresentation
    {
        public string Bone { get; set; }
        public Vector2 Offset { get; set; }
        public float Angle { get; set; }
    }
}