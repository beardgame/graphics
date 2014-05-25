using OpenTK;

namespace amulware.Graphics.Charts
{
    public struct Point2D<T>
    {
        private readonly Vector2d position;
        private readonly T details;

        public Point2D(Vector2d position, T details)
        {
            this.position = position;
            this.details = details;
        }

        public Vector2d Position { get { return this.position; } }
        public T Details { get { return this.details; } }
    }
}
