using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL sampler uniform
    /// </summary>
    public class TextureUniform : SurfaceSetting
    {
        /// <summary>
        /// The <see cref="Texture"/> value of this uniform
        /// </summary>
        public Texture Texture;

        /// <summary>
        /// The <see cref="TextureUnit"/> used by this uniform
        /// </summary>
        public TextureUnit Target;

        /// <summary>
        /// The name of this uniform
        /// </summary>
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureUniform"/> class.
        /// </summary>
        /// <param name="name">The name of this uniform.</param>
        /// <param name="texture">The initial <see cref="Texture"/> value of this uniform.</param>
        /// <param name="target">The initial <see cref="TextureUnit"/> used by this uniform.</param>
        public TextureUniform(string name, Texture texture, TextureUnit target = TextureUnit.Texture0)
        {
            this.name = name;
            this.Target = target;
            this.Texture = texture;
        }

        /// <summary>
        /// Binds the uniform's <see cref="Texture"/> to the specified <see cref="TextureUnit"/> for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            // for explanation on how textures are set, review: http://www.opentk.com/node/2559
            GL.ActiveTexture(this.Target);
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);
            GL.Uniform1(program.GetUniformLocation(this.name), this.Target - TextureUnit.Texture0);
        }
    }
}
