using System;
using System.Net.Sockets;

namespace amulware.Graphics.Animation
{
    sealed public class AnimationSeqence
    {
        public enum Mode
        {
            Loop,
            WaitAtEnd,
            StopAtEnd
        }

        public enum PlayState
        {
            Playing,
            Waiting,
            Stopped
        }

        private readonly AnimationSequenceTemplate template;
        private readonly Mode mode;
        public bool Stopped { get { return this.State == PlayState.Stopped; } }

        public float Duration { get { return this.template.Duration; } }

        public PlayState State { get; private set; }

        private float time;

        private int activeTransitionId;
        private FrameTransition activeTransition;

        public AnimationSeqence(AnimationSequenceTemplate template, Mode mode = Mode.StopAtEnd, PlayState state = PlayState.Playing)
        {
            if (template.Transitions.Count == 0)
                throw new ArgumentException("Cannot animate sequence without keyframes.");

            this.mode = mode;
            this.template = template;
            this.activeTransitionId = 0;
            this.activeTransition = template.Transitions[0];

            this.State = state;
        }

        public float Time { get { return this.time; } }

        public void SetTime(float time)
        {
            float delta = time - this.time;
            this.advanceTime(delta, true);
        }

        private void advanceTime(float delta, bool forceAdvance)
        {
            if (!forceAdvance && this.State != PlayState.Playing)
                return;
            this.time += delta;

            // deal with both negative and positive delta to allow animations to be played backwards
            if (delta < 0)
            {
                while (this.time < this.activeTransition.StartTime)
                {
                    this.activeTransitionId--;
                    if (this.activeTransitionId < 0)
                    {
                        switch (this.mode)
                        {
                            case Mode.Loop:
                                this.time += this.template.Duration;
                                this.activeTransitionId = this.template.Transitions.Count - 1;
                                break;
                            case Mode.WaitAtEnd:
                                this.time = 0;
                                this.State = PlayState.Waiting;
                                return;
                            case Mode.StopAtEnd:
                                this.State = PlayState.Stopped;
                                return;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    this.activeTransition = this.template.Transitions[this.activeTransitionId];
                }
            }
            else
            {
                while (this.time > this.activeTransition.EndTime)
                {
                    this.activeTransitionId++;
                    if (this.activeTransitionId >= this.template.Transitions.Count)
                    {
                        switch (this.mode)
                        {
                            case Mode.Loop:
                                this.time -= this.template.Duration;
                                this.activeTransitionId = 0;
                                break;
                            case Mode.WaitAtEnd:
                                this.time = this.template.Duration;
                                this.State = PlayState.Waiting;
                                return;
                            case Mode.StopAtEnd:
                                this.State = PlayState.Stopped;
                                return;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    this.activeTransition = this.template.Transitions[this.activeTransitionId];
                }
            }
        }

        public void AdvanceTime(float delta)
        {
            this.advanceTime(delta, false);
        }

        private static readonly Func<float, float>[] transitionFuncions =
        {
            x => x,
            x => 0.5f - 0.5f * (float)Math.Cos(Math.PI * x)
        };

        public void ApplyTo(BoneParameters[] parameters)
        {
            if (this.Stopped)
                return;

            if (this.time <= this.activeTransition.DelayEnd)
            {
                if (this.activeTransition.StartFrame != null)
                    this.activeTransition.StartFrame.ApplyTo(parameters, 1);
                return;
            }

            float t = (this.time - this.activeTransition.DelayEnd) / this.activeTransition.Duration;

            t = AnimationSeqence.transitionFuncions[(int)this.activeTransition.Transition](t);

            if (this.activeTransition.StartFrame != null)
                this.activeTransition.StartFrame.ApplyTo(parameters, 1 - t);

            this.activeTransition.EndFrame.ApplyTo(parameters, t);
        }
    }
}
