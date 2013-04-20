using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// This class represents a GLSL Vector2 uniform.
    /// </summary>
    public class Vector2Uniform : SurfaceSetting
    {
        /// <summary>
        /// The name of the uniform
        /// </summary>
        private string name;

        /// <summary>
        /// The <see cref="Vector2"/> value of the uniform
        /// </summary>
        public Vector2 Vector;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        public Vector2Uniform(string name)
            : this(name, Vector2.Zero) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="vector">The initial <see cref="Vector2"/> value of the uniform.</param>
        public Vector2Uniform(string name, Vector2 vector)
        {
            this.name = name;
            this.Vector = vector;
        }


        /// <summary>
        /// Sets the <see cref="Vector2"/> uniform for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.Uniform2(program.GetUniformLocation(this.name), this.Vector);
        }
    }
}
