using System.IO;
using Bearded.Utilities.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    sealed public class ShaderFile : ShaderReloader
    {
        private readonly string filename;
        private readonly string friendlyName;
        private readonly FileModifiedWatcher fileWatcher;

        public ShaderFile(ShaderType type, string filename, string friendlyName)
            : base(type)
        {
            this.filename = filename;
            this.friendlyName = friendlyName;
            this.fileWatcher = new FileModifiedWatcher(filename);
        }
        
        public override bool ChangedSinceLastLoad
        {
            get { return this.fileWatcher.WasModified(false); }
        }

        public string FriendlyName
        {
            get { return this.friendlyName; }
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
