using System;
using System.IO;
using Bearded.Utilities.IO;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.Utilities
{
    sealed class ShaderFile : ReloadableShader
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

    sealed class ShaderSource : ReloadableShader
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
                if (this.source == value)
                    return;
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

    abstract class ReloadableShader : IReloadableShader
    {
        private readonly ShaderType type;
        public ShaderType Type { get { return this.type; } }

        public abstract bool ChangedSinceLastLoad { get; }

        protected ReloadableShader(ShaderType type)
        {
            this.type = type;
        }

        public Shader Load()
        {
            var source = this.getSource();

            switch (this.type)
            {
                case ShaderType.FragmentShader:
                    return new FragmentShader(source);
                case ShaderType.VertexShader:
                    return new VertexShader(source);
                case ShaderType.GeometryShader:
                    return new GeometryShader(source);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract string getSource();
    }

    interface IReloadableShader
    {
        bool ChangedSinceLastLoad { get; }
        ShaderType Type { get; }
        Shader Load();
    }
}
