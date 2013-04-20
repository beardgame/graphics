using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// This class represents a GLSL shader.
    /// </summary>
    public abstract class Shader
    {
        /// <summary>
        /// The GLSL shader object handle.
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader"/> class.
        /// </summary>
        /// <param name="type">The type of shader.</param>
        /// <param name="filename">The file to load the shader from.</param>
        /// <exception cref="System.ApplicationException">Throws an exception of OpenGL reports an error when compiling the shader.</exception>
        public Shader(ShaderType type, string filename)
        {
            this.Handle = GL.CreateShader(type);
            StreamReader streamReader = new StreamReader(filename);
            GL.ShaderSource(this, streamReader.ReadToEnd());
            streamReader.Close();
            GL.CompileShader(this);

            // throw exception if compile failed
            string info;
            int status_code;
            GL.GetShaderInfoLog(this, out info);
            GL.GetShader(this, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            //Console.WriteLine(type.ToString() + " created");
        }

        /// <summary>
        /// Casts the shader object to its GLSL shader object handle, for easy use with OpenGL functions.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <returns>GLSL shader object handle.</returns>
        static public implicit operator int(Shader shader)
        {
            return shader.Handle;
        }
    }
}
