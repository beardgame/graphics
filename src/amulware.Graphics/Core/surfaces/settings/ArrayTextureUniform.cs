using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public class ArrayTextureUniform : SurfaceSetting
    {
        public string Name { get; }
        
        public ArrayTexture Texture { get; set; }

        public TextureUnit Target { get; set; }

        public ArrayTextureUniform(string name, ArrayTexture texture, TextureUnit target = TextureUnit.Texture0)
        {
            Name = name;
            Texture = texture;
            Target = target;
        }

        public override void Set(ShaderProgram program)
        {
            GL.ActiveTexture(Target);
            GL.BindTexture(TextureTarget.Texture2DArray, Texture);
            GL.Uniform1(program.GetUniformLocation(Name), Target - TextureUnit.Texture0);
        }
    }
}
