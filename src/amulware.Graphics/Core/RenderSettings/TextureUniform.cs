using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class TextureUniform : Uniform<Texture>
    {
        public TextureUnit Target { get; }

        public TextureUniform(string name, TextureUnit target, Texture value)
            : base(name, value)
        {
            Target = target;
        }

        protected override void SetAtLocation(int location)
        {
            GL.ActiveTexture(Target);
            using var _ = Value.Bind();
            GL.Uniform1(location, Target - TextureUnit.Texture0);
        }
    }
}
