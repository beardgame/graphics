using amulware.Graphics.Textures;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.RenderSettings
{
    public sealed class ArrayTextureUniform : Uniform<ArrayTexture>
    {
        public TextureUnit Unit { get; }

        public ArrayTextureUniform(string name, TextureUnit unit, ArrayTexture texture)
            : base(name, texture)
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
