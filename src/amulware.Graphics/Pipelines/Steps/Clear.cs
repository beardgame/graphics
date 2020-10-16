using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class Clear<T> : IPipeline
    {
        private readonly ClearBufferMask clearBufferMask;
        private readonly Action<T> setClearValue;
        private readonly T value;

        public Clear(ClearBufferMask clearBufferMask, Action<T> setClearValue, T value)
        {
            this.clearBufferMask = clearBufferMask;
            this.setClearValue = setClearValue;
            this.value = value;
        }

        public void Execute()
        {
            setClearValue(value);
            GL.Clear(clearBufferMask);
        }
    }
}
