using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL shader program.
    /// </summary>
    public class ShaderProgram : IDisposable
    {
        VertexShader vertexShader;
        FragmentShader fragmentShader;

        /// <summary>
        /// The GLSL shader program handle
        /// </summary>
        public readonly int Handle;

        Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
        Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public static ShaderProgram FromFiles(string vertexShaderPath, string fragmentShaderPath)
        {
            return new ShaderProgram(VertexShader.FromFile(vertexShaderPath), FragmentShader.FromFile(fragmentShaderPath));
        }

        public static ShaderProgram FromCode(string vertexShaderCode, string fragmentShaderCode)
        {
            return new ShaderProgram(new VertexShader(vertexShaderCode), new FragmentShader(fragmentShaderCode));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderProgram"/> class.
        /// </summary>
        /// <param name="vs">The vertex shader.</param>
        /// <param name="fs">The fragment shader.</param>
        /// <exception cref="System.ApplicationException">Throws an exception if OpenGL reports an error when linking the linking the shaders to the program.</exception>
        public ShaderProgram(VertexShader vs, FragmentShader fs)
        {
            this.vertexShader = vs;
            this.fragmentShader = fs;

            this.Handle = GL.CreateProgram();

            GL.AttachShader(this, vs);
            GL.AttachShader(this, fs);
            GL.LinkProgram(this);

            // throw exception if linking failed
            string info;
            int status_code;
            GL.GetProgramInfoLog(this, out info);
            GL.GetProgram(this, ProgramParameter.LinkStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            //Console.WriteLine("created shader program");

        }

        /// <summary>
        /// Sets the vertex attributes.
        /// </summary>
        /// <param name="vertexAttributes">The vertex attributes to set.</param>
        public void SetVertexAttributes(VertexAttribute[] vertexAttributes)
        {
            for (int i = 0; i < vertexAttributes.Length; i++)
                vertexAttributes[i].setAttribute(this);
        }

        /// <summary>
        /// Gets an attribute's location.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>The attribute's location, or -1 if not found.</returns>
        public int GetAttributeLocation(string name)
        {
            int i;
            if (!this.attributeLocations.TryGetValue(name, out i))
            {
                i = GL.GetAttribLocation(this, name);
                this.attributeLocations.Add(name, i);
            }
            return i;
        }

        /// <summary>
        /// Gets a uniform's location.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <returns>The uniform's location, or -1 if not found.</returns>
        public int GetUniformLocation(string name)
        {
            int i;
            if (!this.uniformLocations.TryGetValue(name, out i))
            {
                i = GL.GetUniformLocation(this, name);
                this.uniformLocations.Add(name, i);
            }
            return i;
        }


        /// <summary>
        /// Casts the shader program object to its GLSL program object handle, for easy use with OpenGL functions.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>GLSL program object handle.</returns>
        static public implicit operator int(ShaderProgram program)
        {
            return program.Handle;
        }

        #region Disposing

        private bool disposed = false;

        public void Dispose()
        {
            this.dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;
            
            GL.DeleteProgram(this);

            this.disposed = true;
        }

        ~ShaderProgram()
        {
            this.dispose(false);
        }

        #endregion
    }
}
