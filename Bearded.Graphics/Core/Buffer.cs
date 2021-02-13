using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics
{
    public sealed class Buffer<T> : IDisposable where T : struct
    {
        private static readonly int itemSize = Marshal.SizeOf(typeof(T));

        private int handle { get; }

        public int Count { get; private set; }

        public Buffer()
        {
            handle = GL.GenBuffer();
        }

        public Target Bind(BufferTarget target = BufferTarget.ArrayBuffer)
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
