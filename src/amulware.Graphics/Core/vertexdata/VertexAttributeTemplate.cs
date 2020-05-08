using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Represents a template to create a VertexAttribute from.
    /// </summary>
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

        /// <summary>
        /// The size in bytes of this template's attribute.
        /// </summary>
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

        /// <summary>
        /// Creates the attribute from this template given an offset and stride.
        /// </summary>
        /// <param name="offset">
        /// Offset of the attribute in the vertex.
        /// Corresponds to the sum of byte-sizes of preceding attributes.</param>
        /// <param name="stride">
        /// Stride of the vertex of the created attribute.
        /// Corresponds to the sum of byte-sizes of all attributes of the vertex.</param>
        public VertexAttribute ToAttribute(int offset, int stride) =>
            new VertexAttribute(name, size, type, stride, offset, normalize);
    }
}
