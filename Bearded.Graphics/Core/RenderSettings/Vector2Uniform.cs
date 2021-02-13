using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class Vector2Uniform : Uniform<Vector2>
    {
        public Vector2Uniform(string name) : this(name, Vector2.Zero)
        {
        }

        public Vector2Uniform(string name, Vector2 value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform2(location, Value);
        }
    }
}
