using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    public sealed class ShaderSource : ShaderReloader
    {
        private string source;
        private bool changed;

        public ShaderSource(ShaderType type, string source)
            : base(type)
        {
            Source = source;
        }

        public string Source
        {
            set
            {
                source = value;
                changed = true;
            }
        }

        public override bool ChangedSinceLastLoad => changed;

        protected override string GetSource()
        {
            changed = false;
            return source;
        }
    }
}
