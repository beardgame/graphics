namespace Bearded.Graphics.Textures
{
    public interface ITextureTransformation
    {
        void Transform(ref byte[] data, ref int width, ref int height);
    }

    public static class TextureTransformation
    {
        public static ITextureTransformation Premultiply { get; } = new Premultiplication();

        private sealed class Premultiplication : ITextureTransformation
        {
            public void Transform(ref byte[] data, ref int width, ref int height)
            {
                var size = data.Length;
                for (var i = 0; i < size; i += 4)
                {
                    var alpha = data[i + 3] / 255f;
                    data[i] = (byte) (data[i] * alpha);
                    data[i + 1] = (byte) (data[i + 1] * alpha);
                    data[i + 2] = (byte) (data[i + 2] * alpha);
                }
            }
        }
    }
}
