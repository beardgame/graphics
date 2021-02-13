using Bearded.Graphics.Textures;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class TextureUniform : Uniform<Texture>
    {
        public TextureUnit Unit { get; }

        public TextureUniform(string name, TextureUnit unit, Texture value)
            : base(name, value)
        {
            Unit = unit;
        }

        protected override void SetAtLocation(int location)
        {
            GL.ActiveTexture(Unit);
            Value.Bind();
            GL.Uniform1(location, Unit - TextureUnit.Texture0);
        }
    }
}
