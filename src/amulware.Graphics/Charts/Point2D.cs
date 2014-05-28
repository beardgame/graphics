using OpenTK;

namespace amulware.Graphics.Charts
{
    public interface IPoint2D
    {
        Vector2d Position { get; }
    }

    public struct Point2D : IPoint2D
    {
        private readonly Vector2d position;

        public Point2D(Vector2d position)
        {
            this.position = position;
        }

        public Vector2d Position { get { return this.position; } }
    }

    public struct Point2D<T> : IPoint2D
    {
        private readonly Vector2d position;
        private readonly T details;

        public Point2D(Vector2d position)
        {
            this.position = position;
            this.details = default(T);
        }

        public Point2D(Vector2d position, T details)
        {
            this.position = position;
            this.details = details;
        }

        public Vector2d Position { get { return this.position; } }
        public T Details { get { return this.details; } }
    }
}
