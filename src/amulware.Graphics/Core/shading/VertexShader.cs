using System.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL vertex shader.
    /// </summary>
    public sealed class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShader"/> class from source code.
        /// </summary>
        /// <param name="code">The file to load the shader from.</param>
        public VertexShader(string code) : base(ShaderType.VertexShader, code) { }

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
    }
}
