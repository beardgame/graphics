using System.IO;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL fragment shader
    /// </summary>
    public sealed class FragmentShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentShader"/> class from a source file.
        /// </summary>
        /// <param name="filename">Path to the source file.</param>
        public static FragmentShader FromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                var code = streamReader.ReadToEnd();
                return new FragmentShader(code);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentShader"/> class from source code.
        /// </summary>
        /// <param name="sourceCode">Source code of the shader.</param>
        public static FragmentShader FromCode(string sourceCode) => new FragmentShader(sourceCode);
        
        private FragmentShader(string code) : base(ShaderType.FragmentShader, code) { }
    }
}
