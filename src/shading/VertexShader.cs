using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// This class represents a GLSL vertex shader.
    /// </summary>
    public class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShader"/> class.
        /// </summary>
        /// <param name="filename">The file to load the shader from.</param>
        public VertexShader(string filename) : base(ShaderType.VertexShader, filename) { }
    }
}
