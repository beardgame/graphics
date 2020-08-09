using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.RenderSettings
{
    public sealed class Vector2Uniform : Uniform<Vector2>
    {
        public Vector2Uniform(string name, Vector2 value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform2(location, Value);
        }
    }
}
