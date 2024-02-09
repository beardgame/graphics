using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Bearded.Graphics.Shading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Half = OpenTK.Mathematics.Half;

namespace Bearded.Graphics.Vertices;

using PointerType = VertexAttribPointerType;
using Format = VertexAttributeFormat;
using Defaults = (VertexAttribPointerType Type, int Count, int Bytes, VertexAttributeFormat DefaultFormat);

public static class VertexData
{
    public static void SetAttributes<TVertex>(ShaderProgram program)
        where TVertex : struct, IVertexData
        => SetAttributes(TVertex.VertexAttributes, program);

    public static void SetAttributes<TCollection>(TCollection attributes, ShaderProgram program)
        where TCollection : IEnumerable<VertexAttribute>
    {
        foreach (var attribute in attributes)
        {
            attribute.SetAttribute(program);
        }
    }

    public static ImmutableArray<VertexAttribute> MakeAttributeArray(IEnumerable<VertexAttributeTemplate> attributes)
        => MakeAttributeArray(attributes.ToList());

    public static ImmutableArray<VertexAttribute> MakeAttributeArray(params VertexAttributeTemplate[] attributes)
        => MakeAttributeArray((ICollection<VertexAttributeTemplate>)attributes);

    public static ImmutableArray<VertexAttribute> MakeAttributeArray(ICollection<VertexAttributeTemplate> attributes)
    {
        var stride = attributes.Sum(a => a.Bytes);
        var array = ImmutableArray.CreateBuilder<VertexAttribute>(attributes.Count);
        var offset = 0;
        foreach (var template in attributes)
        {
            array.Add(template.ToAttribute(offset, stride));
            offset += template.Bytes;
        }

        return array.MoveToImmutable();
    }

    public static VertexAttributeTemplate MakeAttributeTemplate<T>(string name, Format? format = null)
        => MakeAttributeTemplate(name, typeof(T), format);

    public static VertexAttributeTemplate MakeAttributeTemplate(string name, Type type, Format? format = null)
    {
        if (!knownTypes.TryGetValue(type, out var info))
            throw new ArgumentException($"Unknown type: {type.Name}");

        return MakeAttributeTemplate(name, info.Type, info.Count, info.Bytes, format ?? info.DefaultFormat);
    }

    public static VertexAttributeTemplate MakeAttributeTemplate(
        string name, PointerType type, int numberOfType, int sizeInBytes, Format format)
    {
        if (format == Format.Integer && !isValidIntegerType(type))
            throw new ArgumentException("Invalid type for integer vertex attribute. Must be an integer.");
        if (format == Format.Double && type != PointerType.Double)
            throw new ArgumentException("Invalid type for 64-bit vertex attribute. Must be Double.");

        return new VertexAttributeTemplate(name, numberOfType, sizeInBytes, type, format);
    }

    private static bool isValidIntegerType(PointerType type)
        => type is >= PointerType.Byte and <= PointerType.UnsignedInt;

    private static readonly ReadOnlyDictionary<Type, Defaults> knownTypes
        = new Dictionary<Type, Defaults>
        {
            { typeof(float), (PointerType.Float, 1, 4, Format.Float) },
            { typeof(Vector2), (PointerType.Float, 2, 8, Format.Float) },
            { typeof(Vector3), (PointerType.Float, 3, 12, Format.Float) },
            { typeof(Vector4), (PointerType.Float, 4, 16, Format.Float) },

            { typeof(Half), (PointerType.HalfFloat, 1, 2, Format.Float) },
            { typeof(Vector2h), (PointerType.HalfFloat, 2, 4, Format.Float) },
            { typeof(Vector3h), (PointerType.HalfFloat, 3, 6, Format.Float) },
            { typeof(Vector4h), (PointerType.HalfFloat, 4, 8, Format.Float) },

            { typeof(double), (PointerType.Double, 1, 8, Format.Double) },
            { typeof(Vector2d), (PointerType.Double, 2, 16, Format.Double) },
            { typeof(Vector3d), (PointerType.Double, 3, 24, Format.Double) },
            { typeof(Vector4d), (PointerType.Double, 4, 32, Format.Double) },

            { typeof(byte), (PointerType.UnsignedByte, 1, 1, Format.Integer) },
            { typeof(sbyte), (PointerType.Byte, 1, 1, Format.Integer) },

            { typeof(short), (PointerType.Short, 1, 2, Format.Integer) },
            { typeof(ushort), (PointerType.UnsignedShort, 1, 2, Format.Integer) },

            { typeof(int), (PointerType.Int, 1, 4, Format.Integer) },
            { typeof(uint), (PointerType.UnsignedInt, 1, 4, Format.Integer) },

            { typeof(Color), (PointerType.UnsignedByte, 4, 4, Format.FloatNormalized) },
        }.AsReadOnly();
}
