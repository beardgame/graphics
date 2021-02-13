using System;

namespace Bearded.Graphics
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
        /// The time since the start of the program in seconds
        /// </summary>
        public readonly double TimeInS = 0;

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
        public UpdateEventArgs(double currentTime)
        {
            this.TimeInS = currentTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventArgs"/> class.
        /// </summary>
        /// <param name="lastFrame">The <see cref="UpdateEventArgs"/> instance of the last frame.</param>
        /// <param name="currentTimeInSeconds">The current time.</param>
        public UpdateEventArgs(UpdateEventArgs lastFrame, double currentTimeInSeconds)
        {
            this.Frame = lastFrame.Frame + 1;
            this.ElapsedTimeInS = currentTimeInSeconds - lastFrame.TimeInS;
            this.ElapsedTimeInSf = (float)this.ElapsedTimeInS;

            this.TimeInS = currentTimeInSeconds;
        }
    }
}
