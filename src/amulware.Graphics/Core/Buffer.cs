using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public class Buffer<T> : IDisposable where T : struct
    {
        private static readonly int itemSize = VertexData.SizeOf<T>();

        private int handle { get; }

        public int Count { get; private set; }

        public Buffer()
        {
            handle = GL.GenBuffer();
        }

        public Target Bind(BufferTarget target)
        {
            return new Target(this, target);
        }

        public readonly struct Target : IDisposable
        {
            private const BufferUsageHint defaultUsageHint = BufferUsageHint.StreamDraw;

            private readonly Buffer<T> buffer;
            private readonly BufferTarget target;

            internal Target(Buffer<T> buffer, BufferTarget target)
            {
                this.buffer = buffer;
                this.target = target;

                GL.BindBuffer(target, buffer.handle);
            }

            public void Reserve(int count, BufferUsageHint usageHint = defaultUsageHint)
            {
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));

                bufferData(null, count, target, usageHint);
            }

            public void Upload(T[] data, BufferUsageHint usageHint = defaultUsageHint)
            {
                bufferData(data, data.Length, target, usageHint);
            }

            public void Upload(T[] data, int count, BufferUsageHint usageHint = defaultUsageHint)
            {
                if (count < 0 || count > data.Length)
                    throw new ArgumentOutOfRangeException(nameof(count));

                bufferData(data, count, target, usageHint);
            }

            private void bufferData(T[]? data, int count, BufferTarget target, BufferUsageHint usageHint)
            {
                buffer.Count = count;
                GL.BufferData(target, itemSize * count, data, usageHint);
            }

            public void Dispose()
            {
                GL.BindBuffer(target, 0);
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffer(handle);
        }
    }
}
