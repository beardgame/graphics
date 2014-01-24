namespace amulware.Graphics.Animation
{
    public interface IBoneParameters<TKeyframeParameters>
    {
        void SetToDefault();
        void Add(TKeyframeParameters data, float weight);
        void Add(TKeyframeParameters data1, TKeyframeParameters data2, float data2Weight);

        // also add bone attribute interface
    }
}
