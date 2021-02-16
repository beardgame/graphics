using Bearded.Graphics.Shading;

namespace Bearded.Graphics.RenderSettings
{
    public abstract class Uniform<T> : IRenderSetting
    {
        private readonly string name;

        public T Value { get; set; }

        protected Uniform(string name, T value)
        {
            this.name = name;
            Value = value;
        }

        public void SetForProgram(ShaderProgram program)
        {
            SetAtLocation(program.GetUniformLocation(name));
        }

        public IProgramRenderSetting ForProgram(ShaderProgram program)
        {
            return new ProgramUniform(this, program.GetUniformLocation(name));
        }

        private sealed class ProgramUniform : IProgramRenderSetting
        {
            private readonly Uniform<T> uniform;
            private readonly int location;

            public ProgramUniform(Uniform<T> uniform, int location)
            {
                this.uniform = uniform;
                this.location = location;
            }

            public void Set()
            {
                uniform.SetAtLocation(location);
            }
        }

        protected abstract void SetAtLocation(int location);
    }
}
