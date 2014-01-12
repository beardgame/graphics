namespace amulware.Graphics.Animation
{
    public interface IBoneTransformation<TBoneParameters, TBoneTransformation>
        where TBoneTransformation : IBoneTransformation<TBoneParameters, TBoneTransformation>
    {
        void SetParent(ITransformedBone<TBoneParameters, TBoneTransformation> parent);
        void SetParameters(TBoneParameters parameters);
    }
}
