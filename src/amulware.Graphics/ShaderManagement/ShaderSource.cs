using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    sealed class ShaderSource : ShaderReloader
    {
        private string source;
        private bool changed;

        public ShaderSource(ShaderType type, string source)
            : base(type)
        {
            this.Source = source;
        }

        public string Source
        {
            get { return this.source; }
            set
            {
                this.source = value;
                this.changed = true;
            }
        }

        public override bool ChangedSinceLastLoad
        {
            get { return this.changed; }
        }

        protected override string getSource()
        {
            this.changed = false;
            return this.source;
        }
    }
}
