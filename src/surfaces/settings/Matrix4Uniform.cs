using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// This class represents a GLSL Matrix4 uniform.
    /// </summary>
    public class Matrix4Uniform : SurfaceSetting
    {
        /// <summary>
        /// The name of the uniform
        /// </summary>
        private string name;

        /// <summary>
        /// The <see cref="Matrix4"/> value of the uniform
        /// </summary>
        public Matrix4 Matrix;


        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        public Matrix4Uniform(string name)
            : this(name, Matrix4.Identity) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="matrix">The initial <see cref="Matrix4"/> value of the uniform.</param>
        public Matrix4Uniform(string name, Matrix4 matrix)
        {
            this.name = name;
            this.Matrix = matrix;
        }

        /// <summary>
        /// Sets the <see cref="Matrix4"/> uniform for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        override public void Set(ShaderProgram program)
        {
            GL.UniformMatrix4(program.GetUniformLocation(this.name), false, ref this.Matrix);
        }
    }
}
