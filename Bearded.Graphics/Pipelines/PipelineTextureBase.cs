using System;
using Bearded.Graphics.Textures;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Pipelines
{
    public abstract class PipelineTextureBase : IDisposable
    {
        public Texture Texture { get; }

        protected PipelineTextureBase(Texture texture)
        {
            Texture = texture;
        }

        public void EnsureSize(Vector2i size)
        {
            if (size == new Vector2i(Texture.Width, Texture.Height))
                return;

            using var target = Texture.Bind();
            target.Resize(size.X, size.Y);
        }

        public void Dispose()
        {
            Texture.Dispose();
        }
    }
}
