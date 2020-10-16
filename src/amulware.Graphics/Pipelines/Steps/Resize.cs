using System;
using System.Collections.Immutable;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Pipelines.Steps
{
    sealed class Resize : IPipeline
    {
        private readonly Func<Vector2i> getSize;
        private readonly ImmutableArray<PipelineTextureBase> textures;

        public Resize(Func<Vector2i> getSize, ImmutableArray<PipelineTextureBase> textures)
        {
            this.getSize = getSize;
            this.textures = textures;
        }

        public void Execute()
        {
            var size = getSize();
            foreach (var texture in textures)
            {
                texture.EnsureSize(size);
            }
        }
    }
}
