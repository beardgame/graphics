namespace amulware.Graphics.Animation
{
    public interface IBoneTransformation<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
        where TBoneTransformation : IBoneTransformation<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation>, new()
    {
        void SetBone(Bone<TBoneParameters, TKeyframeParameters, TBoneAttributes, TBoneTransformation> bone);
        void UpdateParameters(TBoneParameters parameters);
    }
}
