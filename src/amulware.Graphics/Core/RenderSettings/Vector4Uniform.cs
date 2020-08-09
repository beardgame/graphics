using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.RenderSettings
{
    public sealed class Vector4Uniform : Uniform<Vector4>
    {
        public Vector4Uniform(string name, Vector4 value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform4(location, Value);
        }
    }
}
