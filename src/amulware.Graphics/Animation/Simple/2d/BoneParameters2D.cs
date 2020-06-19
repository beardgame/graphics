using System.ComponentModel;
using Newtonsoft.Json;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Animation
{
    public struct BoneParameters2D : IBoneParameters<BoneParameters2D>
    {
        public Vector2 Offset { get; set; }
        public float Angle { get; set; }

        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Scale { get; set; }

        public BoneParameters2D (Vector2 offset, float angle, float scale)
            : this()
        {
            this.Offset = offset;
            this.Angle = angle;
            this.Scale = scale;
        }

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

            this.Scale *= data.Scale * weight + (1 - weight);
        }

        public void Add(BoneParameters2D data1, BoneParameters2D data2, float data2Weight)
        {
            float data1Weight = 1 - data2Weight;

            this.Offset += data2.Offset * data2Weight + data1.Offset * data1Weight;
            this.Angle += data2.Angle * data2Weight + data1.Angle * data1Weight;

            this.Scale *= data2.Scale * data2Weight + data1.Scale * data1Weight;
        }
    }
}
