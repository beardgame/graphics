namespace amulware.Graphics.Animation
{
    internal class KeyframeDataJsonRepresentation<TKeyframeParameters>
    {
        public string Bone { get; set; }
        public TKeyframeParameters Parameters { get; set; }
    }
}
