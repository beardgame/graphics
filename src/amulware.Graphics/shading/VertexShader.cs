using System.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL vertex shader.
    /// </summary>
    sealed public class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShader"/> class.
        /// </summary>
        /// <param name="code">The file to load the shader from.</param>
        public VertexShader(string code) : base(ShaderType.VertexShader, code) { }

        public static VertexShader FromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                string code = streamReader.ReadToEnd();
                return new VertexShader(code);
            }
        }
    }
}
