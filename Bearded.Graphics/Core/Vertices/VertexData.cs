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
using Defaults = (VertexAttribPointerType Type, int Count, VertexAttributeFormat DefaultFormat);

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
        => MakeAttributeArray(attributes.ToArray());

    public static ImmutableArray<VertexAttribute> MakeAttributeArray(params VertexAttributeTemplate[] attributes)
        => MakeAttributeArray((ReadOnlySpan<VertexAttributeTemplate>)attributes);

    public static ImmutableArray<VertexAttribute> MakeAttributeArray(ReadOnlySpan<VertexAttributeTemplate> attributes)
    {
        var stride = 0;
        foreach (var template in attributes)
        {
            stride += template.Bytes;
        }
        var array = ImmutableArray.CreateBuilder<VertexAttribute>(attributes.Length);
        var offset = 0;
        foreach (var template in attributes)
        {
            array.Add(template.ToAttribute(offset, stride));
            offset += template.Bytes;
        }

        return array.MoveToImmutable();
    }

    public static VertexAttributeTemplate MakeAttributeTemplate<T>(
        string name, Format? format = null, bool instanced = false)
        => MakeAttributeTemplate(name, typeof(T), format, instanced);

    public static VertexAttributeTemplate MakeAttributeTemplate(
        string name, Type type, Format? format = null, bool instanced = false)
    {
        if (!defaultsForType.TryGetValue(type, out var info))
            throw new ArgumentException($"Unknown type: {type.Name}");

        var bytes = size(info.Type);

        return MakeAttributeTemplate(
            name, info.Type, info.Count, info.Count * bytes, format ?? info.DefaultFormat, instanced);
    }

    public static VertexAttributeTemplate MakeAttributeTemplate(
        string name, PointerType type, int numberOfType, int sizeInBytes, Format format, bool instanced)
    {
        if (format == Format.Integer && !isValidIntegerType(type))
            throw new ArgumentException("Invalid type for integer vertex attribute. Must be an integer.");
        if (format == Format.Double && type != PointerType.Double)
            throw new ArgumentException("Invalid type for 64-bit vertex attribute. Must be Double.");

        return new VertexAttributeTemplate(name, numberOfType, sizeInBytes, type, format, instanced ? 1 : 0);
    }

    private static bool isValidIntegerType(PointerType type)
        => type is >= PointerType.Byte and <= PointerType.UnsignedInt;

    private static readonly ReadOnlyDictionary<Type, Defaults> defaultsForType
        = new Dictionary<Type, Defaults>
        {
            { typeof(float), (PointerType.Float, 1, Format.Float) },
            { typeof(Vector2), (PointerType.Float, 2, Format.Float) },
            { typeof(Vector3), (PointerType.Float, 3, Format.Float) },
            { typeof(Vector4), (PointerType.Float, 4, Format.Float) },

            { typeof(Half), (PointerType.HalfFloat, 1, Format.Float) },
            { typeof(Vector2h), (PointerType.HalfFloat, 2, Format.Float) },
            { typeof(Vector3h), (PointerType.HalfFloat, 3, Format.Float) },
            { typeof(Vector4h), (PointerType.HalfFloat, 4, Format.Float) },

            { typeof(double), (PointerType.Double, 1, Format.Double) },
            { typeof(Vector2d), (PointerType.Double, 2, Format.Double) },
            { typeof(Vector3d), (PointerType.Double, 3, Format.Double) },
            { typeof(Vector4d), (PointerType.Double, 4, Format.Double) },

            { typeof(byte), (PointerType.UnsignedByte, 1, Format.Integer) },
            { typeof(sbyte), (PointerType.Byte, 1, Format.Integer) },

            { typeof(short), (PointerType.Short, 1, Format.Integer) },
            { typeof(ushort), (PointerType.UnsignedShort, 1, Format.Integer) },

            { typeof(int), (PointerType.Int, 1, Format.Integer) },
            { typeof(uint), (PointerType.UnsignedInt, 1, Format.Integer) },

            { typeof(Color), (PointerType.UnsignedByte, 4, Format.FloatNormalized) },
        }.AsReadOnly();

    private static int size(PointerType type)
        => type switch
        {
            PointerType.Byte => 1,
            PointerType.UnsignedByte => 1,
            PointerType.Short => 2,
            PointerType.UnsignedShort => 2,
            PointerType.Int => 4,
            PointerType.UnsignedInt => 4,
            PointerType.Float => 4,
            PointerType.Double => 8,
            PointerType.HalfFloat => 2,
            PointerType.Fixed => 4,
            PointerType.UnsignedInt2101010Rev => 4,
            PointerType.UnsignedInt10F11F11FRev => 4,
            PointerType.Int2101010Rev => 4,
            _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(VertexAttribPointerType)} value: {type}")
        };
}
