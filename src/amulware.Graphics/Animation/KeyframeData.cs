using System.IO;
using OpenTK;

namespace amulware.Graphics.Animation
{
    sealed public class KeyframeData
    {
        private readonly BoneTemplate bone;
        private readonly Vector2 offset;
        private readonly float angle;


        internal KeyframeData(KeyframeDataJsonRepresentation data, SkeletonTemplate skeleton)
        {
            this.offset = data.Offset;
            this.angle = data.Angle;


            this.bone = skeleton[data.Bone];

            if (this.bone == null)
                throw new InvalidDataException("Keyframe data must specify valid bone.");
        }

        public Vector2 Offset
        {
            get { return this.offset; }
        }

        public float Angle
        {
            get { return this.angle; }
        }

        public BoneTemplate Bone
        {
            get { return this.bone; }
        }
    }
}
