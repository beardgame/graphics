using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public struct PostProcessVertexData : IVertexData
    {
        // add attributes and constructors here
        public Vector2 Position;
        public Vector2 TexCoord;

        public PostProcessVertexData(Vector2 position)
        {
            this.Position = position;
            this.TexCoord.X = (position.X + 1) * 0.5f;
            this.TexCoord.Y = (position.Y + 1) * 0.5f;
        }

        public PostProcessVertexData(float x, float y)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.TexCoord.X = (x + 1) * 0.5f;
            this.TexCoord.Y = (y + 1) * 0.5f;
        }

        static private VertexAttribute[] vertexAttributes;

        static private void setVertexAttributes()
        {
            PostProcessVertexData.vertexAttributes = new VertexAttribute[]{
                // add new VertexAttributes here
                new VertexAttribute("v_position", 2, VertexAttribPointerType.Float, 16, 0),
                new VertexAttribute("v_texCoord", 2, VertexAttribPointerType.Float, 16, 8)
            };
        }

        public VertexAttribute[] VertexAttributes()
        {
            if (PostProcessVertexData.vertexAttributes == null)
                PostProcessVertexData.setVertexAttributes();
            return PostProcessVertexData.vertexAttributes;
        }

        public int Size()
        {
            // return size of struct (in bytes) here
            return 16;
        }
    }
}
