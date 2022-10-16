using System;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.ShaderManagement
{
    public sealed class ReloadableShader : IShaderProvider, IDisposable
    {
        private readonly IShaderReloader reloader;

        public ShaderType Type => reloader.Type;

        public Shader Shader { get; private set; }

        public static ReloadableShader LoadFrom(IShaderReloader reloader) => new(reloader);

        private ReloadableShader(IShaderReloader reloader)
        {
            this.reloader = reloader;
            Shader = reloader.Load();
        }

        public bool ReloadIfNeeded()
        {
            if (!reloader.ChangedSinceLastLoad)
                return false;

            Shader.Dispose();
            Shader = reloader.Load();
            return true;
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
