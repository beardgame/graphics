using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ReloadableShader : IShaderProvider
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
                Shader = reloader.Load();
                return true;
            }

            return false;
        }

        public void Reload()
        {
            Shader = reloader.Load();
        }
    }
}
