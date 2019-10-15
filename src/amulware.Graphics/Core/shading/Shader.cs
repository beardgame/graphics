using System;
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
        public int Handle { get; }

        protected Shader(ShaderType type, string code)
        {
            Handle = GL.CreateShader(type);

            GL.ShaderSource(this, code);
            GL.CompileShader(this);

            // throw exception if compile failed
            GL.GetShader(this, ShaderParameter.CompileStatus, out var statusCode);

            if (statusCode == StatusCode.Ok) return;

            GL.GetShaderInfoLog(this, out var info);
            throw new ApplicationException($"Could not load shader: {info}");

        }

        /// <summary>
        /// Casts the shader object to its GLSL shader object handle, for easy use with OpenGL functions.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <returns>GLSL shader object handle.</returns>
        public static implicit operator int(Shader shader) => shader.Handle;

        #region Disposing

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteShader(this);

            disposed = true;
        }

        ~Shader()
        {
            Dispose(false);
        }

        #endregion
    }
}
