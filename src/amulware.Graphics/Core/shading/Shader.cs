using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class Shader : IDisposable
    {
        public int Handle { get; }

        public static Shader Create(ShaderType type, string sourceCode)
        {
            return new Shader(type, sourceCode);
        }

        private Shader(ShaderType type, string code)
        {
            Handle = GL.CreateShader(type);

            GL.ShaderSource(Handle, code);
            GL.CompileShader(Handle);

            throwIfCompilingFailed();
        }

        private void throwIfCompilingFailed()
        {
            GL.GetShader(Handle, ShaderParameter.CompileStatus, out var statusCode);

            if (statusCode == StatusCode.Ok) return;

            GL.GetShaderInfoLog(Handle, out var info);
            throw new ApplicationException($"Could not load shader: {info}");
        }

        public void Dispose()
        {
            GL.DeleteShader(Handle);
        }
    }
}
