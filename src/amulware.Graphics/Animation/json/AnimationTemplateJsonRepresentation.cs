using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    internal class AnimationTemplateJsonRepresentation<TKeyframeParameters, TBoneAttributes>
    {
        public string Name { get; set; }
        public List<BoneJsonRepresentation<TBoneAttributes>> Skeleton { get; set; }
        public List<KeyframeJsonRepresentation<TKeyframeParameters>> Keyframes { get; set; }
        public List<SequenceJsonRepresentation> Sequences { get; set; }
    }
}
