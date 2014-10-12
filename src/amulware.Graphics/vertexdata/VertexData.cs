using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    public static class VertexData
    {
        public static int SizeOf<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }

        #region MakeAttributeArray()
        public static VertexAttribute[] MakeAttributeArray(IList<IAttributeTemplate> attributes)
        {
            var stride = attributes.Sum(a => a.Bytes);
            var array = new VertexAttribute[attributes.Count];
            var offset = 0;
            for (int i = 0; i < attributes.Count; i++)
            {
                var template = attributes[i];
                array[i] = template.ToAttribute(offset, stride);
                offset += template.Bytes;
            }
            return array;
        }

        public static VertexAttribute[] MakeAttributeArray(params IAttributeTemplate[] attributes)
        {
            return MakeAttributeArray((IList<IAttributeTemplate>)attributes);
        }
        public static VertexAttribute[] MakeAttributeArray(IEnumerable<IAttributeTemplate> attributes)
        {
            return MakeAttributeArray(attributes.ToList());
        }
        #endregion

        #region Dictionaries
        private static readonly Dictionary<Type, AttribTypeInfo> knownTypes
        #region init
            = new Dictionary<Type, AttribTypeInfo>
            {
                { typeof(float), VertexAttribPointerType.Float.ToInfo(1, false) },
                { typeof(Vector2), VertexAttribPointerType.Float.ToInfo(2, false) },
                { typeof(Vector3), VertexAttribPointerType.Float.ToInfo(3, false) },
                { typeof(Vector4), VertexAttribPointerType.Float.ToInfo(4, false) },

                { typeof(Half), VertexAttribPointerType.HalfFloat.ToInfo(1, false) },
                { typeof(Vector2h), VertexAttribPointerType.HalfFloat.ToInfo(2, false) },
                { typeof(Vector3h), VertexAttribPointerType.HalfFloat.ToInfo(3, false) },
                { typeof(Vector4h), VertexAttribPointerType.HalfFloat.ToInfo(4, false) },

                { typeof(byte), VertexAttribPointerType.UnsignedByte.ToInfo(1, true) },
                { typeof(sbyte), VertexAttribPointerType.Byte.ToInfo(1, true) },
                
                { typeof(short), VertexAttribPointerType.Short.ToInfo(1, false) },
                { typeof(ushort), VertexAttribPointerType.UnsignedShort.ToInfo(1, false) },

                { typeof(int), VertexAttribPointerType.Int.ToInfo(1, false) },
                { typeof(uint), VertexAttribPointerType.UnsignedInt.ToInfo(1, false) },
                
                { typeof(double), VertexAttribPointerType.Double.ToInfo(1, false) },

                { typeof(Color), VertexAttribPointerType.UnsignedByte.ToInfo(4, true) },
            };
        #endregion

        private static readonly Dictionary<VertexAttribPointerType, int> attribByteSizes
        #region init
            = new Dictionary<VertexAttribPointerType, int>
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
            };
        #endregion

        #endregion

        #region MakeAttributeTemplate()
        public static IAttributeTemplate MakeAttributeTemplate<T>(string name,
            OverridingBool normalize = default(OverridingBool))
        {
            AttribTypeInfo info;
            if (!knownTypes.TryGetValue(typeof(T), out info))
                throw new Exception(string.Format("Unknown type: {0}", typeof(T).Name));

            return MakeAttributeTemplate(name, info.Type, info.Count, normalize.OrDefault(info.DefaultNormalize));
        }

        public static IAttributeTemplate MakeAttributeTemplate(string name, Type type,
            OverridingBool normalize = default(OverridingBool))
        {
            AttribTypeInfo info;
            if(!knownTypes.TryGetValue(type, out info))
                throw new Exception(string.Format("Unknown type: {0}", type.Name));

            return MakeAttributeTemplate(name, info.Type, info.Count, normalize.OrDefault(info.DefaultNormalize));
        }

        public static IAttributeTemplate MakeAttributeTemplate(string name, VertexAttribPointerType type,
            int numberOfType, bool normalize = false)
        {
            return new AttributeTemplate(name, numberOfType, type, normalize);
        }
        #endregion

        #region Types
        public interface IAttributeTemplate
        {
            int Bytes { get; }
            VertexAttribute ToAttribute(int offset, int stride);
        }

        struct AttribTypeInfo
        {
            public AttribTypeInfo(VertexAttribPointerType type, int count, bool defaultNormalize)
                : this()
            {
                this.Type = type;
                this.Count = count;
                this.DefaultNormalize = defaultNormalize;
            }

            public VertexAttribPointerType Type { get; private set; }
            public int Count { get; private set; }
            public bool DefaultNormalize { get; private set; }
        }

        private static AttribTypeInfo ToInfo(this VertexAttribPointerType type, int count, bool defaultNormalize)
        {
            return new AttribTypeInfo(type, count, defaultNormalize);
        }

        sealed class AttributeTemplate : IAttributeTemplate
        {
            private readonly int bytes;
            private readonly string name;
            private readonly int size;
            private readonly VertexAttribPointerType type;
            private readonly bool normalize;

            public AttributeTemplate(string name, int size, VertexAttribPointerType type, bool normalize)
            {
                int bytes;
                if (!attribByteSizes.TryGetValue(type, out bytes))
                    throw new Exception(string.Format("Unknown VertexAttribPointerType: {0}", type));
                this.bytes = bytes * size;
                this.name = name;
                this.size = size;
                this.type = type;
                this.normalize = normalize;
            }

            public int Bytes { get { return this.bytes; } }

            public VertexAttribute ToAttribute(int offset, int stride)
            {
                return new VertexAttribute(this.name, this.size, this.type, stride, offset, this.normalize);
            }
        }
        #endregion
    }
}
