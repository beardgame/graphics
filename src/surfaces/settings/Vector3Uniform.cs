using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL Vector3 uniform.
    /// </summary>
    public class Vector3Uniform : SurfaceSetting
    {
        /// <summary>
        /// The name of the uniform
        /// </summary>
        private string name;

        /// <summary>
        /// The <see cref="Vector3"/> value of the uniform
        /// </summary>
        public Vector3 Vector;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        public Vector3Uniform(string name)
            : this(name, Vector3.Zero) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="vector">The initial <see cref="Vector3"/> value of the uniform.</param>
        public Vector3Uniform(string name, Vector3 vector)
        {
            this.name = name;
            this.Vector = vector;
        }


        /// <summary>
        /// Sets the <see cref="Vector3"/> uniform for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.Uniform3(program.GetUniformLocation(this.name), this.Vector);
        }
    }
}
