using System.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL fragment shader
    /// </summary>
    sealed public class FragmentShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentShader"/> class from source code.
        /// </summary>
        /// <param name="code">The file to load the shader from.</param>
        public FragmentShader(string code) : base(ShaderType.FragmentShader, code) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentShader"/> class from a source file.
        /// </summary>
        /// <param name="filename">Path to the source file.</param>
        public static FragmentShader FromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                string code = streamReader.ReadToEnd();
                return new FragmentShader(code);
            }
        }
    }
}
