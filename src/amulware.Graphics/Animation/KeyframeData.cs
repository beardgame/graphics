using System.IO;

namespace amulware.Graphics.Animation
{
    sealed public class KeyframeData<TKeyframeParameters, TBoneAttributes>
    {
        private readonly BoneTemplate<TBoneAttributes> bone;
        private readonly TKeyframeParameters parameters;

        internal KeyframeData(KeyframeDataJsonRepresentation<TKeyframeParameters> data, SkeletonTemplate<TBoneAttributes> skeleton)
        {
            this.bone = skeleton[data.Bone];

            this.parameters = data.Parameters;

            if (this.bone == null)
                throw new InvalidDataException("Keyframe data must specify valid bone.");
        }

        public BoneTemplate<TBoneAttributes> Bone
        {
            get { return this.bone; }
        }

        public TKeyframeParameters Parameters
        {
            get { return this.parameters; }
        }
    }
}
