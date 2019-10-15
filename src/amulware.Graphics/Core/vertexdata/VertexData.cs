using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class contains helper types and methods to easily create vertex attribute layouts.
    /// </summary>
    public static class VertexData
    {
        /// <summary>
        /// Returns the size in bytes of a given type.
        /// </summary>
        /// <typeparam name="T">The type to return the size of.</typeparam>
        public static int SizeOf<T>() => Marshal.SizeOf(typeof(T));

        #region MakeAttributeArray()
        /// <summary>
        /// Creates a <see cref="VertexAttribute"/> array from a list of attribute templates.
        /// Offset and stride are calculated automatically, assuming zero padding.
        /// </summary>
        /// <param name="attributes">The attribute templates.</param>
        public static VertexAttribute[] MakeAttributeArray(IList<IAttributeTemplate> attributes)
        {
            var stride = attributes.Sum(a => a.Bytes);
            var array = new VertexAttribute[attributes.Count];
            var offset = 0;
            for (var i = 0; i < attributes.Count; i++)
            {
                var template = attributes[i];
                array[i] = template.ToAttribute(offset, stride);
                offset += template.Bytes;
            }
            return array;
        }

        /// <summary>
        /// Creates a <see cref="VertexAttribute"/> array from a list of attribute templates.
        /// Offset and stride are calculated automatically, assuming zero padding.
        /// </summary>
        /// <param name="attributes">The attribute templates.</param>
        public static VertexAttribute[] MakeAttributeArray(params IAttributeTemplate[] attributes) =>
            MakeAttributeArray((IList<IAttributeTemplate>) attributes);

        /// <summary>
        /// Creates a <see cref="VertexAttribute"/> array from a list of attribute templates.
        /// Offset and stride are calculated automatically, assuming zero padding.
        /// </summary>
        /// <param name="attributes">The attribute templates.</param>
        public static VertexAttribute[] MakeAttributeArray(IEnumerable<IAttributeTemplate> attributes) =>
            MakeAttributeArray(attributes.ToList());

        #endregion

        #region Dictionaries
        private static readonly ImmutableDictionary<Type, AttributeTypeInfo> knownTypes
        #region init
            = ImmutableDictionary.CreateRange(new Dictionary<Type, AttributeTypeInfo>
            {
                { typeof(float), toInfo(VertexAttribPointerType.Float, 1, defaultNormalize: false) },
                { typeof(Vector2), toInfo(VertexAttribPointerType.Float, 2, defaultNormalize: false) },
                { typeof(Vector3), toInfo(VertexAttribPointerType.Float, 3, defaultNormalize: false) },
                { typeof(Vector4), toInfo(VertexAttribPointerType.Float, 4, defaultNormalize: false) },

                { typeof(Half), toInfo(VertexAttribPointerType.HalfFloat, 1, defaultNormalize: false) },
                { typeof(Vector2h), toInfo(VertexAttribPointerType.HalfFloat, 2, defaultNormalize: false) },
                { typeof(Vector3h), toInfo(VertexAttribPointerType.HalfFloat, 3, defaultNormalize: false) },
                { typeof(Vector4h), toInfo(VertexAttribPointerType.HalfFloat, 4, defaultNormalize: false) },

                { typeof(double), toInfo(VertexAttribPointerType.Double, 1, defaultNormalize: false) },
                { typeof(Vector2d), toInfo(VertexAttribPointerType.HalfFloat, 2, defaultNormalize: false) },
                { typeof(Vector3d), toInfo(VertexAttribPointerType.HalfFloat, 3, defaultNormalize: false) },
                { typeof(Vector4d), toInfo(VertexAttribPointerType.HalfFloat, 4, defaultNormalize: false) },

                { typeof(byte), toInfo(VertexAttribPointerType.UnsignedByte, 1, defaultNormalize: true) },
                { typeof(sbyte), toInfo(VertexAttribPointerType.Byte, 1, defaultNormalize: true) },

                { typeof(short), toInfo(VertexAttribPointerType.Short, 1, defaultNormalize: false) },
                { typeof(ushort), toInfo(VertexAttribPointerType.UnsignedShort, 1, defaultNormalize: false) },

                { typeof(int), toInfo(VertexAttribPointerType.Int, 1, defaultNormalize: false) },
                { typeof(uint), toInfo(VertexAttribPointerType.UnsignedInt, 1, defaultNormalize: false) },

                { typeof(Color), toInfo(VertexAttribPointerType.UnsignedByte, 4, defaultNormalize: true) },
            });
        #endregion

        private static readonly ImmutableDictionary<VertexAttribPointerType, int> attribByteSizes
        #region init
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
        #endregion

        #endregion

        #region MakeAttributeTemplate()
        /// <summary>
        /// Creates a vertex attribute template of a given basic type with and a given name.
        /// </summary>
        /// <param name="name">The name of the attribute in shader code.</param>
        /// <param name="normalize">
        /// Whether to normalize the attribute.
        /// For null, only attributes of type <see cref="byte"/>, <see cref="sbyte"/>, and <see cref="Color"/> are normalised.
        /// Default is null.
        /// </param>
        /// <typeparam name="T">
        /// The type the attribute.
        /// Supported are all signed and unsigned integer types, float, double and <see cref="Half"/>,
        /// three and four dimensional vectors of all three floating point types, and <see cref="Color"/>.
        /// </typeparam>
        /// <exception cref="ArgumentException">The given type is not supported.</exception>
        public static IAttributeTemplate MakeAttributeTemplate<T>(string name, bool? normalize = null) =>
            MakeAttributeTemplate(name, typeof (T), normalize);

        /// <summary>
        /// Creates a vertex attribute template of a given basic type with and a given name.
        /// </summary>
        /// <param name="name">The name of the attribute in shader code.</param>
        /// <param name="type">
        /// The type the attribute.
        /// Supported are all signed and unsigned integer types, float, double and <see cref="Half"/>,
        /// three and four dimensional vectors of all three floating point types, and <see cref="Color"/>.
        /// </param>
        /// <param name="normalize">
        /// Whether to normalize the attribute.
        /// For null, only attributes of type <see cref="byte"/>, <see cref="sbyte"/>, and <see cref="Color"/> are normalised.
        /// Default is null.
        /// </param>
        /// <exception cref="ArgumentException">The given type is not supported.</exception>
        public static IAttributeTemplate MakeAttributeTemplate(string name, Type type, bool? normalize = null)
        {
            if(!knownTypes.TryGetValue(type, out var info))
                throw new ArgumentException($"Unknown type: {type.Name}");

            return MakeAttributeTemplate(name, info.Type, info.Count, normalize ?? info.DefaultNormalize);
        }

        /// <summary>
        /// Creates a vertex attribute template of a given basic type with and a given name.
        /// </summary>
        /// <param name="name">The name of the attribute in shader code.</param>
        /// <param name="type">The <see cref="VertexAttribPointerType"/> of the attribute.</param>
        /// <param name="numberOfType">Number of components of the given type in this attribute.</param>
        /// <param name="normalize">Whether to normalize the attribute.</param>
        public static IAttributeTemplate MakeAttributeTemplate(
                string name, VertexAttribPointerType type, int numberOfType, bool normalize = false) =>
            new AttributeTemplate(name, numberOfType, type, normalize);

        #endregion

        #region Types
        /// <summary>
        /// Represents a template to create a VertexAttribute from.
        /// </summary>
        public interface IAttributeTemplate
        {
            /// <summary>
            /// The size in bytes of this template's attribute.
            /// </summary>
            int Bytes { get; }
            /// <summary>
            /// Creates the attribute from this template given an offset and stride.
            /// </summary>
            /// <param name="offset">
            /// Offset of the attribute in the vertex.
            /// Corresponds to the sum of byte-sizes of preceding attributes.</param>
            /// <param name="stride">
            /// Stride of the vertex of the created attribute.
            /// Corresponds to the sum of byte-sizes of all attributes of the vertex.</param>
            VertexAttribute ToAttribute(int offset, int stride);
        }

        private struct AttributeTypeInfo
        {
            public VertexAttribPointerType Type { get; }
            public int Count { get; }
            public bool DefaultNormalize { get; }

            public AttributeTypeInfo(VertexAttribPointerType type, int count, bool defaultNormalize)
                : this()
            {
                Type = type;
                Count = count;
                DefaultNormalize = defaultNormalize;
            }
        }

        private static AttributeTypeInfo toInfo(VertexAttribPointerType type, int count, bool defaultNormalize) =>
            new AttributeTypeInfo(type, count, defaultNormalize);

        private sealed class AttributeTemplate : IAttributeTemplate
        {
            private readonly string name;
            private readonly int size;
            private readonly VertexAttribPointerType type;
            private readonly bool normalize;

            public int Bytes { get; }

            public AttributeTemplate(string name, int size, VertexAttribPointerType type, bool normalize)
            {
                if (!attribByteSizes.TryGetValue(type, out var bytes))
                    throw new ArgumentException($"Unknown VertexAttribPointerType: {type}");

                Bytes = bytes * size;
                this.name = name;
                this.size = size;
                this.type = type;
                this.normalize = normalize;
            }

            public VertexAttribute ToAttribute(int offset, int stride) =>
                new VertexAttribute(name, size, type, stride, offset, normalize);
        }
        #endregion
    }
}
