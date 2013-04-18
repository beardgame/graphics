using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class TextureUniform : SurfaceSetting
    {
        public Texture Texture;
        public TextureUnit Target;
        private string name;

        public TextureUniform(string name, Texture texture, TextureUnit target = TextureUnit.Texture0)
        {
            this.name = name;
            this.Target = target;
            this.Texture = texture;
        }

        public override void Set(ShaderProgram program)
        {
            // for explanation on how textures are set, review: http://www.opentk.com/node/2559
            GL.ActiveTexture(this.Target);
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);
            GL.Uniform1(program.GetUniformLocation(this.name), this.Target - TextureUnit.Texture0);
        }
    }
}
