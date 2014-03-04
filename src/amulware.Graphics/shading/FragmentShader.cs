using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL fragment shader
    /// </summary>
    sealed public class FragmentShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentShader"/> class.
        /// </summary>
        /// <param name="filename">The file to load the shader from.</param>
        public FragmentShader(string code) : base(ShaderType.FragmentShader, code) { }

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
