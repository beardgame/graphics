
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    sealed class ReloadableShader
    {
        private readonly IShaderReloader reloader;

        private Shader shader;

        public ReloadableShader(IShaderReloader reloader)
        {
            this.reloader = reloader;
        }

        public ShaderType Type
        {
            get { return this.reloader.Type; }
        }

        public Shader Shader
        {
            get { return this.shader; }
        }

        public bool TryReload()
        {
            if (!this.reloader.ChangedSinceLastLoad)
                return false;

            this.shader = this.reloader.Load();
            return true;
        }

    }
}
