using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSequenceTemplate
    {
        private readonly string name;
        private readonly ReadOnlyCollection<FrameTransition> transitions;
        private readonly float duration;

        internal AnimationSequenceTemplate(SequenceJsonRepresentation json, Dictionary<string, Keyframe> keyframes)
        {
            if (string.IsNullOrEmpty(json.Name))
                throw new InvalidDataException("Keyframe must have name.");
            this.name = json.Name;

            if (json.Transitions == null)
            {
                this.transitions = new List<FrameTransition>().AsReadOnly();
                return;
            }

            var transitions = new List<FrameTransition>();
            FrameTransition lastTransition = null;
            foreach (var t in json.Transitions)
            {
                lastTransition = new FrameTransition(t, keyframes, lastTransition);
                transitions.Add(lastTransition);
                this.duration = lastTransition.EndTime;
            }
            this.transitions = transitions.AsReadOnly();
        }

        public string Name { get { return this.name; } }
        public ReadOnlyCollection<FrameTransition> Transitions { get { return this.transitions; } }

        public float Duration { get { return this.duration; } }
    }
}
