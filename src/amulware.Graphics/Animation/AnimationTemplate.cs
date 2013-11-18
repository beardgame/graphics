using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationTemplate
    {
        private readonly string name;
        private readonly SkeletonTemplate skeletonTemplate;
        private readonly Dictionary<string, Keyframe> keyframes;
        private readonly Dictionary<string, AnimationSequence> sequences;

        private readonly Keyframe baseFrame;

        public AnimationTemplate(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
