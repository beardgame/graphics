using System;

namespace amulware.Graphics
{
    /// <summary>
    /// This class is a time keeping device used to facilitate properly timed main loop updates.
    /// </summary>
    public class UpdateEventArgs : EventArgs
    {
        /// <summary>
        /// The frame count, counting every update since the start of the program.
        /// </summary>
        public readonly int Frame = 0;

        /// <summary>
        /// The time since the start of the program in milliseconds
        /// </summary>
        public readonly int TimeInMs = 0;
        /// <summary>
        /// The time since the start of the program in seconds
        /// </summary>
        public readonly double TimeInS = 0;

        /// <summary>
        /// The elapsed time since the last update in milliseconds
        /// </summary>
        public readonly int ElapsedTimeInMs = 0;
        /// <summary>
        /// The elapsed time ince the last update in seconds
        /// </summary>
        public readonly double ElapsedTimeInS = 0;

        /// <summary>
        /// The elapsed time ince the last update in seconds
        /// </summary>
        public readonly float ElapsedTimeInSf = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventArgs"/> class.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public UpdateEventArgs(int currentTime)
        {
            this.TimeInMs = currentTime;
            this.TimeInS = currentTime * 0.001;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventArgs"/> class.
        /// </summary>
        /// <param name="lastFrame">The <see cref="UpdateEventArgs"/> instance of the last frame.</param>
        /// <param name="currentTime">The current time.</param>
        public UpdateEventArgs(UpdateEventArgs lastFrame, int currentTime)
        {
            this.Frame = lastFrame.Frame + 1;
            this.ElapsedTimeInMs = currentTime - lastFrame.TimeInMs;
            this.ElapsedTimeInS = this.ElapsedTimeInMs * 0.001;
            this.ElapsedTimeInS = (float)this.ElapsedTimeInS;

            this.TimeInMs = currentTime;
            this.TimeInS = currentTime * 0.001;
        }
    }
}
