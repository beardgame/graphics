using System;

namespace Bearded.Graphics.Textures;

public interface IBindableTexture<out TTarget>
    where TTarget : struct, IDisposable
{
    TTarget Bind();
}
