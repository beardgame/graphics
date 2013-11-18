using OpenTK;

namespace amulware.Graphics.Animation
{
    public struct BoneParameters
    {
        public Vector2 Offset { get; set; }
        public float Angle { get; set; }

        public BoneParameters(Vector2 offset, float angle)
            : this()
        {
            this.Offset = offset;
            this.Angle = angle;
        }

        public BoneParameters(KeyframeData data)
            : this(data.Offset, data.Angle)
        {
        }

        public void Add(KeyframeData data, float weight)
        {
            this.Offset += data.Offset * weight;
            this.Angle += data.Angle * weight;
        }
    }
}
