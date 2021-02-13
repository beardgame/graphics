using System;
using System.Collections.Immutable;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class Resize<TState> : IPipeline<TState>
    {
        private readonly Func<TState, Vector2i> getSize;
        private readonly ImmutableArray<PipelineTextureBase> textures;

        public Resize(Func<TState, Vector2i> getSize, ImmutableArray<PipelineTextureBase> textures)
        {
            this.getSize = getSize;
            this.textures = textures;
        }

        public void Execute(TState state)
        {
            var size = getSize(state);
            foreach (var texture in textures)
            {
                texture.EnsureSize(size);
            }
        }
    }
}
