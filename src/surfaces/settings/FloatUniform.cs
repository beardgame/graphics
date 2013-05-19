using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL float uniform.
    /// </summary>
    public class FloatUniform : SurfaceSetting
    {
        /// <summary>
        /// The name of the uniform
        /// </summary>
        private string name;

        /// <summary>
        /// The <see cref="float"/> value of the uniform
        /// </summary>
        public float Float;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatUniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        public FloatUniform(string name)
            : this(name, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatUniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="f">The initial <see cref="float"/> value of the uniform.</param>
        public FloatUniform(string name, float f)
        {
            this.name = name;
            this.Float = f;
        }

        /// <summary>
        /// Sets the <see cref="float"/> uniform for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.Uniform1(program.GetUniformLocation(this.name), this.Float);
        }
    }
}
