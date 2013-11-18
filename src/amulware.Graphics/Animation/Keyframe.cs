using System.Collections.Generic;

namespace amulware.Graphics.Animation
{
    sealed public class Keyframe
    {
        private readonly string name;
        private readonly List<KeyframeData> data;

        public Keyframe(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
