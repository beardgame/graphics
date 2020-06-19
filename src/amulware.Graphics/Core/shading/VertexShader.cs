using System.IO;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL vertex shader.
    /// </summary>
    public sealed class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShader"/> class from a source file.
        /// </summary>
        /// <param name="filename">Path to the source file.</param>
        public static VertexShader FromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                var code = streamReader.ReadToEnd();
                return new VertexShader(code);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShader"/> class from source code.
        /// </summary>
        /// <param name="sourceCode">Source code of the shader.</param>
        public static VertexShader FromCode(string sourceCode) => new VertexShader(sourceCode);
        
        private VertexShader(string code) : base(ShaderType.VertexShader, code) { }
    }
}
