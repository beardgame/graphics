using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class ArrayTextureUniform : Uniform<ArrayTexture>
    {
        public TextureUnit Target { get; }

        public ArrayTextureUniform(string name, TextureUnit target, ArrayTexture texture)
            : base(name, texture)
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
