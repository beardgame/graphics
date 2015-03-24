using System.IO;
using Bearded.Utilities.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    sealed class ShaderFile : ShaderReloader
    {
        private readonly string filename;
        private readonly FileModifiedWatcher fileWatcher;

        public ShaderFile(ShaderType type, string filename)
            : base(type)
        {
            this.filename = filename;
            this.fileWatcher = new FileModifiedWatcher(filename);
        }
        
        public override bool ChangedSinceLastLoad
        {
            get { return this.fileWatcher.WasModified(false); }
        }

        protected override string getSource()
        {
            this.fileWatcher.Reset();
            using (var reader = new StreamReader(this.filename))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
