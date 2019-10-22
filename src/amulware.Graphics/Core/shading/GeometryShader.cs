using System.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL geometry shader.
    /// </summary>
    public sealed class GeometryShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryShader"/> class from a source file.
        /// </summary>
        /// <param name="filename">Path to the source file.</param>
        public static GeometryShader FromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                var code = streamReader.ReadToEnd();
                return new GeometryShader(code);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryShader"/> class from source code.
        /// </summary>
        /// <param name="sourceCode">Source code of the shader.</param>
        public static GeometryShader FromCode(string sourceCode) => new GeometryShader(sourceCode);
        
        private GeometryShader(string code) : base(ShaderType.GeometryShader, code) { }
    }
}
