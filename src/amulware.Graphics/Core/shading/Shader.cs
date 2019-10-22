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

            throwIfCompilingFailed();
        }

        private void throwIfCompilingFailed()
        {
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

        public void Dispose()
        {
            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteShader(this);
        }
    }
}
