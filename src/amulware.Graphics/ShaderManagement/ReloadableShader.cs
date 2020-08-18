using System;
using amulware.Graphics.Shading;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReloadableShader : IShaderProvider, IDisposable
    {
        private readonly IShaderReloader reloader;

        public ShaderType Type => reloader.Type;

        public Shader Shader { get; private set; }

        public static ReloadableShader LoadFrom(IShaderReloader reloader) => new ReloadableShader(reloader);

        private ReloadableShader(IShaderReloader reloader)
        {
            this.reloader = reloader;
            Shader = reloader.Load();
        }

        public bool ReloadIfNeeded()
        {
            if (reloader.ChangedSinceLastLoad)
            {
                Shader?.Dispose();
                Shader = reloader.Load();
                return true;
            }

            return false;
        }

        public void Reload()
        {
            Shader = reloader.Load();
        }

        public void Dispose()
        {
            Shader.Dispose();
        }
    }
}
