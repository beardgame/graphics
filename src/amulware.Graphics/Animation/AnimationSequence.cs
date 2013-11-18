using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSequence
    {
        private readonly string name;
        private readonly List<FrameTransition> transitions;

        public AnimationSequence(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
