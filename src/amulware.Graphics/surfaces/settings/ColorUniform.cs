using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL Color uniform.
    /// </summary>
    public class ColorUniform : SurfaceSetting
    {
        /// <summary>
        /// The name of the uniform
        /// </summary>
        private string name;

        /// <summary>
        /// The <see cref="Color"/> value of the uniform
        /// </summary>
        public Color Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorUniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        public ColorUniform(string name)
            : this(name, Color.White) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorUniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="color">The initial <see cref="Color"/> value of the uniform.</param>
        public ColorUniform(string name, Color color)
        {
            this.name = name;
            this.Color = color;
        }


        /// <summary>
        /// Sets the <see cref="Color"/> uniform for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.Uniform4(program.GetUniformLocation(this.name), this.Color.ToRGBAVector());
        }
    }
}
