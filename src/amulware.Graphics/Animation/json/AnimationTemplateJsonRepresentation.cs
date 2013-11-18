using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    internal class AnimationTemplateJsonRepresentation
    {
        public string Name { get; set; }
        public List<BoneJsonRepresentation> Skeleton { get; set; }
        public List<KeyframeJsonRepresentation> Keyframes { get; set; }
        public List<SequenceJsonRepresentation> Sequences { get; set; }
    }
}
