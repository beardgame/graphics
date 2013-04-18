using System;

namespace AWGraphics
{
    public class UpdateEventArgs : EventArgs
    {
        public readonly int Frame = 0;

        public readonly int TimeInMs = 0;
        public readonly double TimeInS = 0;

        public readonly int ElapsedTimeInMs = 0;
        public readonly double ElapsedTimeInS = 0;

        public UpdateEventArgs(int currentTime)
        {
            this.TimeInMs = currentTime;
            this.TimeInS = currentTime * 0.001;
        }

        public UpdateEventArgs(UpdateEventArgs lastFrame, int currentTime)
        {
            this.Frame = lastFrame.Frame + 1;
            this.ElapsedTimeInMs = currentTime - lastFrame.TimeInMs;
            this.ElapsedTimeInS = this.ElapsedTimeInMs * 0.001;

            this.TimeInMs = currentTime;
            this.TimeInS = currentTime * 0.001;
        }
    }
}
