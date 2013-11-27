using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace amulware.Graphics.surfaces
{
    internal interface IVertexAttributeProvider<TVertexData>
        where TVertexData : struct, IVertexData
    {
        void SetVertexData();
    }
}
