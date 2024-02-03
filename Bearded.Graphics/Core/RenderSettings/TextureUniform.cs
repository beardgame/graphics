using System;
using Bearded.Graphics.Textures;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.RenderSettings;

public sealed class TextureUniform(string name, TextureUnit unit, Texture value)
    : Uniform<Texture, Texture.Target>(name, unit, value);

public sealed class ArrayTextureUniform(string name, TextureUnit unit, ArrayTexture value)
    : Uniform<ArrayTexture, ArrayTexture.Target>(name, unit, value);

public abstract class Uniform<TTexture, TTarget> : Uniform<TTexture>
    where TTexture : IBindableTexture<TTarget>
    where TTarget : struct, IDisposable
{
    public TextureUnit Unit { get; }

    internal Uniform(string name, TextureUnit unit, TTexture value)
        : base(name, value)
    {
        Unit = unit;
    }

    protected override void SetAtLocation(int location)
    {
        GL.ActiveTexture(Unit);
        Value.Bind();
        GL.Uniform1(location, Unit - TextureUnit.Texture0);
    }
}
