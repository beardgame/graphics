namespace amulware.Graphics.Animation
{
    public interface ITransformedBone<TBoneParameters, TBoneTransformation>
        where TBoneTransformation : IBoneTransformation<TBoneParameters, TBoneTransformation>
    {
        TBoneTransformation Transformation { get; }
    }

    sealed public class Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>
        : ITransformedBone<TBoneParameters, TBoneTransformation>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
        where TKeyframeParameters : IKeyframeParameters
        where TBoneTransformation : IBoneTransformation<TBoneParameters, TBoneTransformation>, new()
    {
        private readonly Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> parent;
        public Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> Parent { get { return this.parent; } }

        private readonly BoneTemplate<TBoneAttributes> template;
        public BoneTemplate<TBoneAttributes> Template { get { return this.template; } }

        private TBoneTransformation transformation = new TBoneTransformation();

        public Bone(
            Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> parent,
            BoneTemplate<TBoneAttributes> template
            )
        {
            this.parent = parent;
            this.template = template;
            this.transformation.SetParent(parent);
        }

        public TBoneTransformation Transformation { get { return this.transformation; } }

        public void SetParameters(TBoneParameters parameters)
        {
            this.transformation.SetParameters(parameters);
        }

    }
}
