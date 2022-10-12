using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Vertices
{
    public sealed class VertexAttributeTemplate
    {
        private static readonly ImmutableDictionary<VertexAttribPointerType, int> attributeByteSizes
            = ImmutableDictionary.CreateRange(new Dictionary<VertexAttribPointerType, int>
            {
                { VertexAttribPointerType.Byte, 1 },
                { VertexAttribPointerType.UnsignedByte, 1 },
                { VertexAttribPointerType.Short, 2 },
                { VertexAttribPointerType.UnsignedShort, 2 },
                { VertexAttribPointerType.HalfFloat, 2 },
                { VertexAttribPointerType.Int, 4 },
                { VertexAttribPointerType.UnsignedInt, 4 },
                { VertexAttribPointerType.Float, 4 },
                { VertexAttribPointerType.Double, 8 },
            });

        private readonly string name;
        private readonly int size;
        private readonly VertexAttribPointerType type;
        private readonly bool normalize;

        public int Bytes { get; }

        internal VertexAttributeTemplate(string name, int size, VertexAttribPointerType type, bool normalize)
        {
            if (!attributeByteSizes.TryGetValue(type, out var bytes))
                throw new ArgumentException($"Unknown VertexAttribPointerType: {type}");

            Bytes = bytes * size;
            this.name = name;
            this.size = size;
            this.type = type;
            this.normalize = normalize;
        }

        public VertexAttribute ToAttribute(int offset, int stride) => new(name, size, type, stride, offset, normalize);
    }
}
