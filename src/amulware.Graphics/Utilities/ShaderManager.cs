namespace amulware.Graphics.Utilities
{
    sealed class ShaderManager
    {
        private readonly string basePath;

        public ShaderManager(string basePath)
        {
            this.basePath = basePath;
        }

        public ISurfaceShader this[string name]
        {
            get { return null; }
        }
    }
}
