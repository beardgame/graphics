using OpenTK;

namespace amulware.Graphics.Animation
{
    sealed public class KeyframeData
    {
        private readonly BoneTemplate boneTemplate;
        private readonly Vector2 offset;
        private readonly float angle;

        public KeyframeData(BoneTemplate boneTemplate, Vector2 offset, float angle)
        {
            this.offset = offset;
            this.angle = angle;
            this.boneTemplate = boneTemplate;
        }

        public Vector2 Offset
        {
            get { return this.offset; }
        }

        public float Angle
        {
            get { return this.angle; }
        }

        public BoneTemplate BoneTemplate
        {
            get { return this.boneTemplate; }
        }
    }
}
