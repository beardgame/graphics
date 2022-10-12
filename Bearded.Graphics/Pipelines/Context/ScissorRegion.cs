using System;
using System.Drawing;

namespace Bearded.Graphics.Pipelines.Context
{
    public readonly struct ScissorRegion : IEquatable<ScissorRegion>
    {
        public Rectangle? Rectangle { get; }

        public static ScissorRegion FullTarget => new();
        public static ScissorRegion Single(Rectangle rectangle) => new(rectangle);
        public static ScissorRegion SingleOrFullTarget(Rectangle? rectangle) => new(rectangle);

        private ScissorRegion(Rectangle? rectangle)
        {
            Rectangle = rectangle;
        }

        public bool Equals(ScissorRegion other)
        {
            return Nullable.Equals(Rectangle, other.Rectangle);
        }

        public override bool Equals(object? obj)
        {
            return obj is ScissorRegion other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Rectangle.GetHashCode();
        }

        public static bool operator ==(ScissorRegion left, ScissorRegion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScissorRegion left, ScissorRegion right)
        {
            return !left.Equals(right);
        }
    }
}
