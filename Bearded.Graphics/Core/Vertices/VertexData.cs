using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Vertices
{
    /// <summary>
    /// This class contains helper types and methods to easily create vertex attribute layouts.
    /// </summary>
    public static class VertexData
    {
        public static void SetAttributes<TVertex>(ShaderProgram program)
            where TVertex : struct, IVertexData
        {
            SetAttributes(default(TVertex).VertexAttributes, program);
        }

        public static void SetAttributes(VertexAttribute[] attributes, ShaderProgram program)
        {
            foreach (var attribute in attributes)
            {
                attribute.SetAttribute(program);
            }
        }

        /// <summary>
        /// Creates a <see cref="VertexAttribute"/> array from a list of attribute templates.
        /// Offset and stride are calculated automatically, assuming zero padding.
        /// </summary>
        /// <param name="attributes">The attribute templates.</param>
        public static VertexAttribute[] MakeAttributeArray(IList<VertexAttributeTemplate> attributes)
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
        public static VertexAttribute[] MakeAttributeArray(params VertexAttributeTemplate[] attributes) =>
            MakeAttributeArray((IList<VertexAttributeTemplate>) attributes);

        /// <summary>
        /// Creates a <see cref="VertexAttribute"/> array from a list of attribute templates.
        /// Offset and stride are calculated automatically, assuming zero padding.
        /// </summary>
        /// <param name="attributes">The attribute templates.</param>
        public static VertexAttribute[] MakeAttributeArray(IEnumerable<VertexAttributeTemplate> attributes) =>
            MakeAttributeArray(attributes.ToList());

        private static readonly ImmutableDictionary<Type, (VertexAttribPointerType Type, int Count, bool DefaultNormalize)> knownTypes
            = ImmutableDictionary.CreateRange(new Dictionary<Type, (VertexAttribPointerType, int, bool)>
            {
                { typeof(float), (VertexAttribPointerType.Float, 1, false) },
                { typeof(Vector2), (VertexAttribPointerType.Float, 2, false) },
                { typeof(Vector3), (VertexAttribPointerType.Float, 3, false) },
                { typeof(Vector4), (VertexAttribPointerType.Float, 4, false) },

                { typeof(OpenTK.Mathematics.Half), (VertexAttribPointerType.HalfFloat, 1, false) },
                { typeof(Vector2h), (VertexAttribPointerType.HalfFloat, 2, false) },
                { typeof(Vector3h), (VertexAttribPointerType.HalfFloat, 3, false) },
                { typeof(Vector4h), (VertexAttribPointerType.HalfFloat, 4, false) },

                { typeof(double), (VertexAttribPointerType.Double, 1, false) },
                { typeof(Vector2d), (VertexAttribPointerType.HalfFloat, 2, false) },
                { typeof(Vector3d), (VertexAttribPointerType.HalfFloat, 3, false) },
                { typeof(Vector4d), (VertexAttribPointerType.HalfFloat, 4, false) },

                { typeof(byte), (VertexAttribPointerType.UnsignedByte, 1, true) },
                { typeof(sbyte), (VertexAttribPointerType.Byte, 1, true) },

                { typeof(short), (VertexAttribPointerType.Short, 1, false) },
                { typeof(ushort), (VertexAttribPointerType.UnsignedShort, 1, false) },

                { typeof(int), (VertexAttribPointerType.Int, 1, false) },
                { typeof(uint), (VertexAttribPointerType.UnsignedInt, 1, false) },

                { typeof(Color), (VertexAttribPointerType.UnsignedByte, 4, true) },
            });

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
        public static VertexAttributeTemplate MakeAttributeTemplate<T>(string name, bool? normalize = null) =>
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
        public static VertexAttributeTemplate MakeAttributeTemplate(string name, Type type, bool? normalize = null)
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
        public static VertexAttributeTemplate MakeAttributeTemplate(
                string name, VertexAttribPointerType type, int numberOfType, bool normalize = false) =>
            new(name, numberOfType, type, normalize);
    }
}
