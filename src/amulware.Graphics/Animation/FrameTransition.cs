using System.Collections.Generic;
using System.IO;

namespace amulware.Graphics.Animation
{
    sealed public class FrameTransition
    {
        internal enum TransitionType
        {
            Linear
        }

        public float StartTime { get; private set; }
        public float EndTime { get; private set; }
        public Keyframe StartFrame { get; private set; }
        public Keyframe EndFrame { get; private set; }
        public float Duration { get; private set; }
        public float Delay { get; private set; }
        public float DelayEnd { get; private set; }
        private TransitionType transition;

        internal FrameTransition(TransitionJsonRepresentation json, Dictionary<string, Keyframe> keyframes, FrameTransition preceedingFrame)
        {
            if (json.Delay < 0)
                throw new InvalidDataException("Frame transition delay must be non-negative.");

            if (json.Time < 0)
                throw new InvalidDataException("Frame transition time must be non-negative.");

            this.Duration = json.Time;
            this.Delay = json.Delay;
            this.transition = json.Transition;

            this.StartTime = preceedingFrame == null ? 0 : preceedingFrame.EndTime;
            this.StartFrame = preceedingFrame == null ? null : preceedingFrame.EndFrame;

            this.DelayEnd = this.StartTime + this.Delay;

            this.EndTime = this.StartTime + this.Delay + this.Duration;

            Keyframe endFrame;
            if(!keyframes.TryGetValue(json.Frame, out endFrame))
                throw new InvalidDataException("Frame transition must have valid key frame.");

            this.EndFrame = endFrame;
        }
    }
}
