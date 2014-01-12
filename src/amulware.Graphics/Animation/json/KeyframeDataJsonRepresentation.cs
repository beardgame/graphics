using OpenTK;

namespace amulware.Graphics.Animation
{
    internal class KeyframeDataJsonRepresentation<TKeyframeParameters>
        where TKeyframeParameters : IKeyframeParameters
    {
        public string Bone { get; set; }
        public TKeyframeParameters Parameters { get; set; }
    }
}
