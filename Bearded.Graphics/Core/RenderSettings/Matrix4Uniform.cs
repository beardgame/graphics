using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class Matrix4Uniform : Uniform<Matrix4>
    {
        public Matrix4Uniform(string name) : this(name, Matrix4.Identity)
        {
        }

        public Matrix4Uniform(string name, Matrix4 value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            var value = Value;
            GL.UniformMatrix4(location, false, ref value);
        }
    }
}
