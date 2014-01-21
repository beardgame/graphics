using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
    {
        private readonly string name;
        private readonly ReadOnlyCollection<FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>> transitions;
        private readonly float duration;

        internal AnimationSequenceTemplate(
            SequenceJsonRepresentation json,
            Dictionary<string, Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>> keyframes
            )
        {
            if (string.IsNullOrEmpty(json.Name))
                throw new InvalidDataException("Keyframe must have name.");
            this.name = json.Name;

            if (json.Transitions == null)
            {
                this.transitions = new List<FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>>().AsReadOnly();
                return;
            }

            var transitions = new List<FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>>();
            FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes> lastTransition = null;
            foreach (var t in json.Transitions)
            {
                lastTransition = new FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>(t, keyframes, lastTransition);
                transitions.Add(lastTransition);
                this.duration = lastTransition.EndTime;
            }
            this.transitions = transitions.AsReadOnly();
        }

        public string Name { get { return this.name; } }
        public ReadOnlyCollection<FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>> Transitions { get { return this.transitions; } }

        public float Duration { get { return this.duration; } }
    }
}
