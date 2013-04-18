using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public struct SimpleSpriteVertexData : IVertexData
    {
        public Vector3 Position; // 12 bytes
        public Vector2 TexCoord; // 8 bytes
        public Color Color; // 4 bytes
        public Vector2 Expand; // 8 bytes

        static private VertexAttribute[] vertexAttributes;

        public SimpleSpriteVertexData(Vector3 position, Vector2 uv, Color color, Vector2 expand)
        {
            this.Position = position;
            this.TexCoord = uv;
            this.Color = color;
            this.Expand = expand;
        }

        public SimpleSpriteVertexData(Vector3 position, Vector2 uv, Color color, float expandX, float expandY)
        {
            this.Position = position;
            this.TexCoord = uv;
            this.Color = color;
            this.Expand.X = expandX;
            this.Expand.Y = expandY;
        }

        static private void setVertexAttributes()
        {
            SimpleSpriteVertexData.vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, 32, 0),
                new VertexAttribute("v_texcoord", 2, VertexAttribPointerType.Float, 32, 12),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, 32, 20, true),
                new VertexAttribute("v_expand", 2, VertexAttribPointerType.Float, 32, 24),
            };
        }

        public VertexAttribute[] VertexAttributes()
        {
            if (SimpleSpriteVertexData.vertexAttributes == null)
                SimpleSpriteVertexData.setVertexAttributes();
            return SimpleSpriteVertexData.vertexAttributes;
        }

        public int Size()
        {
            return 32;
        }
    }
}
