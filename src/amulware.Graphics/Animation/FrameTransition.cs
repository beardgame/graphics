namespace amulware.Graphics.Animation
{
    sealed public class FrameTransition
    {
        private enum TransitionType
        {
            Linear
        }

        private float startTime;
        private float endTime;
        private Keyframe startFrame;
        private Keyframe endFrame;
        private float duration;
        private float delay;
        private TransitionType transition;
    }
}
