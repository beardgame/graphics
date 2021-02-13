using System;
using System.Collections.Generic;

namespace amulware.Graphics.Rendering
{
    public interface IBatchedRenderable
    {
        public event Action<IRenderable>? BatchActivated;
        public event Action<IRenderable>? BatchDeactivated;

        IEnumerable<IRenderable> GetActiveBatches();
    }
}
