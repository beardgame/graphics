using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class ColorUniform : Uniform<Color>
    {
        public ColorUniform(string name) : this(name, Color.White)
        {
        }

        public ColorUniform(string name, Color value) : base(name, value)
        {
        }

        protected override void SetAtLocation(int location)
        {
            GL.Uniform4(location, Value.AsRGBAVector);
        }
    }
}
