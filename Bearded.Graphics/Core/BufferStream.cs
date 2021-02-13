using System;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics
{
    public sealed class BufferStream<T> where T : struct
    {
        public Buffer<T> Buffer { get; }

        private T[] data;

        public int Capacity => data.Length;
        public int Count { get; private set; }
        public bool IsDirty { get; private set; }

        public BufferStream(Buffer<T> buffer, int capacity = 0)
        {
            Buffer = buffer;
            data = new T[capacity > 0 ? capacity : 4];
        }

        public void FlushIfDirty(BufferTarget target = BufferTarget.ArrayBuffer)
        {
            if (!IsDirty)
                return;

            Flush(target);
        }

        public void Flush(BufferTarget target)
        {
            using var t = Buffer.Bind(target);
            t.Upload(data, Count);
            IsDirty = false;
        }

        // TODO(#24): measure performance impact of using 'in' keyword
        public void Add(T item)
        {
            var newCount = Count + 1;
            ensureCapacity(newCount);
            data[Count] = item;
            Count = newCount;
            IsDirty = true;
        }

        public void Add(T item0, T item1)
        {
            var newCount = Count + 2;
            ensureCapacity(newCount);
            data[Count] = item0;
            data[Count + 1] = item1;
            Count = newCount;
            IsDirty = true;
        }

        public void Add(T item0, T item1, T item2)
        {
            var newCount = Count + 3;
            ensureCapacity(newCount);
            data[Count] = item0;
            data[Count + 1] = item1;
            data[Count + 2] = item2;
            Count = newCount;
            IsDirty = true;
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
            IsDirty = true;
        }

        public void Add(T[] items)
        {
            var newCount = Count + items.Length;
            ensureCapacity(newCount);
            Array.Copy(items, 0, data, Count, items.Length);
            Count = newCount;
            IsDirty = true;
        }

        public Span<T> AddRange(int count)
        {
            var newCount = Count + count;
            ensureCapacity(newCount);
            var span = data.AsSpan(Count, count);
            Count = newCount;
            IsDirty = true;
            return span;
        }

        private void ensureCapacity(int minCapacity)
        {
            if (Capacity <= minCapacity)
                Array.Resize(ref data, Math.Max(data.Length * 2, minCapacity));
        }

        public void Clear()
        {
            if (Count == 0)
                return;

            Count = 0;
            IsDirty = true;
        }
    }
}
