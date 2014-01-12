using System.Collections.Generic;
using System.IO;

namespace amulware.Graphics.Animation
{
    internal static class FrameTransition
    {
        internal enum TransitionType
        {
            Linear = 0,
            Smooth = 1
        }
    }

    sealed public class FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
        where TKeyframeParameters : IKeyframeParameters
    {

        public float StartTime { get; private set; }
        public float EndTime { get; private set; }
        public Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> StartFrame { get; private set; }
        public Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> EndFrame { get; private set; }
        public float Duration { get; private set; }
        public float Delay { get; private set; }
        public float DelayEnd { get; private set; }
        internal FrameTransition.TransitionType Transition { get; private set; }

        internal FrameTransition(
            TransitionJsonRepresentation json,
            Dictionary<string, Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes>> keyframes,
            FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes> preceedingFrame
            )
        {
            if (json.Delay < 0)
                throw new InvalidDataException("Frame transition delay must be non-negative.");

            if (json.Time < 0)
                throw new InvalidDataException("Frame transition time must be non-negative.");

            this.Duration = json.Time;
            this.Delay = json.Delay;
            this.Transition = json.Transition;

            this.StartTime = preceedingFrame == null ? 0 : preceedingFrame.EndTime;
            this.StartFrame = preceedingFrame == null ? null : preceedingFrame.EndFrame;

            this.DelayEnd = this.StartTime + this.Delay;

            this.EndTime = this.StartTime + this.Delay + this.Duration;

            Keyframe<TBoneParameters, TKeyframeParameters, TBoneAttributes> endFrame;
            if(!keyframes.TryGetValue(json.Frame, out endFrame))
                throw new InvalidDataException("Frame transition must have valid key frame.");

            this.EndFrame = endFrame;
        }
    }
}
