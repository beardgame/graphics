using System;

namespace amulware.Graphics.Animation
{
    public static class AnimationSequence
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

        internal static readonly Func<float, float>[] TransitionFuncions =
        {
            x => x,
            x => 0.5f - 0.5f * (float)Math.Cos(Math.PI * x)
        };
    }

    sealed public class AnimationSequence<TBoneParameters, TKeyframeParameters, TBoneAttributes>
        where TBoneParameters : struct, IBoneParameters<TKeyframeParameters>
    {

        private readonly AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> template;
        private readonly AnimationSequence.Mode mode;
        public bool Stopped { get { return this.State == AnimationSequence.PlayState.Stopped; } }

        public float Duration { get { return this.template.Duration; } }

        public AnimationSequence.PlayState State { get; private set; }

        private float time;

        private int activeTransitionId;
        private FrameTransition<TBoneParameters, TKeyframeParameters, TBoneAttributes> activeTransition;

        public AnimationSequence(
            AnimationSequenceTemplate<TBoneParameters, TKeyframeParameters, TBoneAttributes> template,
            AnimationSequence.Mode mode = AnimationSequence.Mode.StopAtEnd,
            AnimationSequence.PlayState state = AnimationSequence.PlayState.Playing
            )
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
            if (!forceAdvance && this.State != AnimationSequence.PlayState.Playing)
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
                            case AnimationSequence.Mode.Loop:
                                this.time += this.template.Duration;
                                this.activeTransitionId = this.template.Transitions.Count - 1;
                                break;
                            case AnimationSequence.Mode.WaitAtEnd:
                                this.time = 0;
                                this.State = AnimationSequence.PlayState.Waiting;
                                return;
                            case AnimationSequence.Mode.StopAtEnd:
                                this.State = AnimationSequence.PlayState.Stopped;
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
                            case AnimationSequence.Mode.Loop:
                                this.time -= this.template.Duration;
                                this.activeTransitionId = 0;
                                break;
                            case AnimationSequence.Mode.WaitAtEnd:
                                this.time = this.template.Duration;
                                this.State = AnimationSequence.PlayState.Waiting;
                                return;
                            case AnimationSequence.Mode.StopAtEnd:
                                this.State = AnimationSequence.PlayState.Stopped;
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

        public void ApplyTo(TBoneParameters[] parameters)
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

            t = AnimationSequence.TransitionFuncions[(int)this.activeTransition.Transition](t);

            this.activeTransition.ApplyTo(parameters, t);
        }
    }
}
