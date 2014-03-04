using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL shader.
    /// </summary>
    public abstract class Shader : IDisposable
    {
        /// <summary>
        /// The GLSL shader object handle.
        /// </summary>
        public readonly int Handle;

        public Shader(ShaderType type, string code)
        {
            this.Handle = GL.CreateShader(type);

            GL.ShaderSource(this, code);
            GL.CompileShader(this);

            // throw exception if compile failed
            string info;
            int status_code;
            GL.GetShaderInfoLog(this, out info);
            GL.GetShader(this, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

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
            
            GL.DeleteShader(this);

            this.disposed = true;
        }

        ~Shader()
        {
            this.dispose(false);
        }

        #endregion
    }
}
