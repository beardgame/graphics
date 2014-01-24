namespace amulware.Graphics.Animation
{
    sealed public class Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
        where TBoneTransformation : IBoneTransformation<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>, new()
    {
        private readonly Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> parent;
        public Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> Parent { get { return this.parent; } }

        private readonly BoneTemplate<TBoneAttributes> template;
        public BoneTemplate<TBoneAttributes> Template { get { return this.template; } }

        private readonly TBoneTransformation transformation = new TBoneTransformation();

        public Bone(
            Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> parent,
            BoneTemplate<TBoneAttributes> template
            )
        {
            this.parent = parent;
            this.template = template;
            this.transformation.SetBone(parent);
        }

        public TBoneTransformation Transformation { get { return this.transformation; } }

        public void UpdateParameters(TBoneParameters parameters)
        {
            this.transformation.UpdateParameters(parameters);
        }

    }
}
