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
        /// Initializes a new instance of the <see cref="VertexShader"/> class.
        /// </summary>
        /// <param name="code">The file to load the shader from.</param>
        public GeometryShader(string code) : base(ShaderType.GeometryShader, code) { }

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
