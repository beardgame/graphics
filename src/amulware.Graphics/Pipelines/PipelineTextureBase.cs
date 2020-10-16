using amulware.Graphics.Textures;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Pipelines
{
    public abstract class PipelineTextureBase
    {
        public Texture Texture { get; }

        protected PipelineTextureBase(Texture texture)
        {
            this.Texture = texture;
        }

        public void EnsureSize(Vector2i size)
        {
            if (size == new Vector2i(Texture.Width, Texture.Height))
                return;

            using var target = Texture.Bind();
            target.Resize(size.X, size.Y);
        }
    }
}