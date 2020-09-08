using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.RenderSettings
{
    public sealed class FloatUniform : Uniform<float>
    {
        public FloatUniform(string name, float value = 0) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform1(location, Value);
        }
    }
}
