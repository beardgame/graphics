using System.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL geometry shader.
    /// </summary>
    sealed public class GeometryShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryShader"/> class from source code.
        /// </summary>
        /// <param name="code">The file to load the shader from.</param>
        public GeometryShader(string code) : base(ShaderType.GeometryShader, code) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryShader"/> class from a source file.
        /// </summary>
        /// <param name="filename">Path to the source file.</param>
        public static GeometryShader FromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                string code = streamReader.ReadToEnd();
                return new GeometryShader(code);
            }
        }
    }
}
