namespace amulware.Graphics.Animation
{
    public interface IBoneParameters<TKeyframeParameters>
        where TKeyframeParameters : IKeyframeParameters
    {
        void SetToDefault();
        void Add(TKeyframeParameters data, float weight);
        // create overload with two keyframes
        // and maybe a 'add root parameters' method, just in case?

        // also add bone attribute interface
    }
}
