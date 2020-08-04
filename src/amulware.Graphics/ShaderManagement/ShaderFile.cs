using System.IO;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ShaderFile : IShaderReloader
    {
        private readonly string filename;
        private readonly FileModifiedWatcher fileWatcher;

        public string FriendlyName { get; }

        public ShaderType Type { get; }
        public bool ChangedSinceLastLoad => fileWatcher.WasModified(false);

        public ShaderFile(ShaderType type, string filename, string friendlyName)
        {
            Type = type;
            this.filename = filename;
            FriendlyName = friendlyName;
            fileWatcher = FileModifiedWatcher.FromPath(filename);
        }

        public Shader Load()
        {
            fileWatcher.Reset();
            var source = File.ReadAllText(filename);

            return Shader.Create(Type, source);
        }
    }
}
