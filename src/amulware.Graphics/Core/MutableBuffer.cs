using System;
using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public class MutableBuffer<T> : IDisposable where T : struct
    {
        private readonly Buffer<T> buffer;

        private T[] data;
        private bool isDirty;

        public int Capacity => data.Length;
        public int Count { get; private set; }

        public MutableBuffer(int capacity = 0)
        {
            buffer = new Buffer<T>();
            data = new T[capacity > 0 ? capacity : 4];
        }

        public Target Bind(BufferTarget target = BufferTarget.ElementArrayBuffer)
        {
            return new Target(this, target);
        }

        public readonly struct Target : IDisposable
        {
            private readonly MutableBuffer<T> buffer;
            private readonly Buffer<T>.Target bufferTarget;

            internal Target(MutableBuffer<T> self, BufferTarget target)
            {
                buffer = self;
                bufferTarget = self.buffer.Bind(target);
            }

            public void UploadIfNeeded()
            {
                if (!buffer.isDirty)
                    return;

                bufferTarget.Upload(buffer.data, buffer.Count);
                buffer.isDirty = false;
            }

            public void Dispose()
            {
                bufferTarget.Dispose();
            }
        }

        public void Add(T item)
        {
            var newCount = Count + 1;
            ensureCapacity(newCount);
            data[Count] = item;
            Count = newCount;
            isDirty = true;
        }

        public void Add(T item0, T item1)
        {
            var newCount = Count + 2;
            ensureCapacity(newCount);
            data[Count] = item0;
            data[Count + 1] = item1;
            Count = newCount;
            isDirty = true;
        }

        public void Add(T item0, T item1, T item2)
        {
            var newCount = Count + 3;
            ensureCapacity(newCount);
            data[Count] = item0;
            data[Count + 1] = item1;
            data[Count + 2] = item2;
            Count = newCount;
            isDirty = true;
        }

        public void Add(T item0, T item1, T item2, T item3)
        {
            var newCount = Count + 4;
            ensureCapacity(newCount);
            data[Count] = item0;
            data[Count + 1] = item1;
            data[Count + 2] = item2;
            data[Count + 3] = item3;
            Count = newCount;
            isDirty = true;
        }

        public void Add(params T[] items)
        {
            var newCount = Count + items.Length;
            ensureCapacity(newCount);
            Array.Copy(items, 0, data, Count, items.Length);
            Count = newCount;
            isDirty = true;
        }

        public Span<T> AddRange(int count)
        {
            var newCount = Count + count;
            ensureCapacity(newCount);
            var span = data.AsSpan(Count, count);
            Count = newCount;
            isDirty = true;
            return span;
        }

        private void ensureCapacity(int minCapacity)
        {
            if (Capacity <= minCapacity)
                Array.Resize(ref data, Math.Max(data.Length * 2, minCapacity));
        }

        public void Clear()
        {
            Count = 0;
            isDirty = true;
        }

        public void Dispose()
        {
            buffer.Dispose();
        }
    }
}
