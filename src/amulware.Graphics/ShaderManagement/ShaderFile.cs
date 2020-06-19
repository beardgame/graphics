using System.IO;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ShaderFile : ShaderReloader
    {
        private readonly string filename;
        private readonly FileModifiedWatcher fileWatcher;

        public string FriendlyName { get; }

        public override bool ChangedSinceLastLoad => fileWatcher.WasModified(false);

        public ShaderFile(ShaderType type, string filename, string friendlyName)
            : base(type)
        {
            this.filename = filename;
            FriendlyName = friendlyName;
            fileWatcher = new FileModifiedWatcher(filename);
        }

        protected override string GetSource()
        {
            fileWatcher.Reset();
            return File.ReadAllText(filename);
        }
    }
}
