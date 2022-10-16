using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Pipelines.Context
{
    public readonly struct DepthMode : IEquatable<DepthMode>
    {
        public bool Write { get; }
        public DepthFunction TestFunction { get; }

        public static DepthMode Disable => new(false, DepthFunction.Always);
        public static DepthMode Default => new(true, DepthFunction.Less);
        public static DepthMode WriteOnly => new(true, DepthFunction.Always);
        public static DepthMode WriteWithTest(DepthFunction test) => new(true, test);
        public static DepthMode TestOnly(DepthFunction test) => new(false, test);

        private DepthMode(bool write, DepthFunction testFunction)
        {
            Write = write;
            TestFunction = testFunction;
        }

        public bool Equals(DepthMode other)
        {
            return Write == other.Write && TestFunction == other.TestFunction;
        }

        public override bool Equals(object? obj)
        {
            return obj is DepthMode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Write, (int) TestFunction);
        }

        public static bool operator ==(DepthMode left, DepthMode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DepthMode left, DepthMode right)
        {
            return !left.Equals(right);
        }
    }
}
