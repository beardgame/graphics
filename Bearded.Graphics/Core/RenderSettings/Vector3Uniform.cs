using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class Vector3Uniform : Uniform<Vector3>
    {
        public Vector3Uniform(string name) : this(name, Vector3.Zero)
        {
        }

        public Vector3Uniform(string name, Vector3 value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform3(location, Value);
        }
    }
}
