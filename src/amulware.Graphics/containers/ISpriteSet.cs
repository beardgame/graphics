using System;
using System.Collections.Generic;

namespace amulware.Graphics
{
    interface ISpriteSet<TVertexData>
        where TVertexData : struct, IVertexData
    {
        Sprite<TVertexData> this[string spriteName] { get; }
    }
}
