using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class Vector4Uniform : Uniform<Vector4>
    {
        public Vector4Uniform(string name) : this(name, Vector4.Zero)
        {
        }

        public Vector4Uniform(string name, Vector4 value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform4(location, Value);
        }
    }
}
