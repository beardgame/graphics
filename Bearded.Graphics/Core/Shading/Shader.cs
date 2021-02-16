using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Shading
{
    public sealed class Shader : IDisposable
    {
        public ShaderType Type { get; }
        public int Handle { get; }

        public static Shader Create(ShaderType type, string sourceCode)
        {
            return new Shader(type, sourceCode);
        }

        private Shader(ShaderType type, string code)
        {
            Type = type;
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
