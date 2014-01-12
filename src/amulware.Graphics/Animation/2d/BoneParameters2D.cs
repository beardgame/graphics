using OpenTK;

namespace amulware.Graphics.Animation
{
    public struct BoneParameters2D : IBoneParameters<BoneParameters2D>, IKeyframeParameters
    {
        public Vector2 Offset { get; set; }
        public float Angle { get; set; }
        public float Scale { get; set; }

        public void SetToDefault()
        {
            this.Offset = Vector2.Zero;
            this.Angle = 0;
            this.Scale = 1;
        }

        public void Add(BoneParameters2D data, float weight)
        {
            this.Offset += data.Offset * weight;
            this.Angle += data.Angle * weight;

            this.Scale += data.Scale * weight;
        }
    }
}
