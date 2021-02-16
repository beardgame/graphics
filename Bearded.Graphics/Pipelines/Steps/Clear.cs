using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class Clear<TState, T> : IPipeline<TState>
    {
        private readonly ClearBufferMask clearBufferMask;
        private readonly System.Action<T> setClearValue;
        private readonly T value;

        public Clear(ClearBufferMask clearBufferMask, System.Action<T> setClearValue, T value)
        {
            this.clearBufferMask = clearBufferMask;
            this.setClearValue = setClearValue;
            this.value = value;
        }

        public void Execute(TState state)
        {
            setClearValue(value);
            GL.Clear(clearBufferMask);
        }
    }
}
