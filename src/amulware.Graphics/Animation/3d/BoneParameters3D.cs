using System.ComponentModel;
using Newtonsoft.Json;
using OpenTK;

namespace amulware.Graphics.Animation
{
    public struct BoneParameters3D : IBoneParameters<BoneParameters3D>
    {
        public Vector3 Offset { get; set; }
        public float AngleX { get; set; }
        public float AngleY { get; set; }
        public float AngleZ { get; set; }

        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Scale { get; set; }

        public BoneParameters3D(Vector3 offset, float angleX, float angleY, float angleZ, float scale)
            : this()
        {
            this.Offset = offset;
            this.AngleX = angleX;
            this.AngleY = angleY;
            this.AngleZ = angleZ;
            this.Scale = scale;
        }

        public void SetToDefault()
        {
            this.Offset = Vector3.Zero;
            this.AngleX = 0;
            this.AngleY = 0;
            this.AngleY = 0;
            this.Scale = 1;
        }

        public void Add(BoneParameters3D data, float weight)
        {
            this.Offset += data.Offset * weight;
            this.AngleX += data.AngleX * weight;
            this.AngleY += data.AngleY * weight;
            this.AngleZ += data.AngleZ * weight;

            this.Scale *= data.Scale * weight + (1 - weight);
        }

        public void Add(BoneParameters3D data1, BoneParameters3D data2, float data2Weight)
        {
            float data1Weight = 1 - data2Weight;

            this.Offset += data2.Offset * data2Weight + data1.Offset * data1Weight;
            this.AngleX += data2.AngleX * data2Weight + data1.AngleX * data1Weight;
            this.AngleY += data2.AngleY * data2Weight + data1.AngleY * data1Weight;
            this.AngleZ += data2.AngleZ * data2Weight + data1.AngleZ * data1Weight;

            this.Scale *= data2.Scale * data2Weight + data1.Scale * data1Weight;
        }
    }
}
