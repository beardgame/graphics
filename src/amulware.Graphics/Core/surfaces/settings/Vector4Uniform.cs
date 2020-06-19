using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL Vector4 uniform.
    /// </summary>
    public class Vector4Uniform : SurfaceSetting
    {
        /// <summary>
        /// The name of the uniform
        /// </summary>
        private string name;

        /// <summary>
        /// The <see cref="Vector4"/> value of the uniform
        /// </summary>
        public Vector4 Vector;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        public Vector4Uniform(string name)
            : this(name, Vector4.Zero) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4Uniform"/> class.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="vector">The initial <see cref="Vector4"/> value of the uniform.</param>
        public Vector4Uniform(string name, Vector4 vector)
        {
            this.name = name;
            this.Vector = vector;
        }


        /// <summary>
        /// Sets the <see cref="Vector4"/> uniform for a shader program. Is called before the draw call.
        /// </summary>
        /// <param name="program">The program.</param>
        public override void Set(ShaderProgram program)
        {
            GL.Uniform4(program.GetUniformLocation(this.name), this.Vector);
        }
    }
}
