using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public struct PrimitiveVertexData : IVertexData
    {

        readonly Vector3 position; // 12 bytes
        readonly Color color; // 4 bytes

        public PrimitiveVertexData(float x, float y, float z, Color color)
        {
            this.position.X = x;
            this.position.Y = y;
            this.position.Z = z;
            this.color = color;
        }

        public PrimitiveVertexData(Vector3 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        static private VertexAttribute[] vertexAttributes;

        static private void setVertexAttributes()
        {
            PrimitiveVertexData.vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, 16, 0),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, 16, 12, true)
            };
        }

        public VertexAttribute[] VertexAttributes()
        {
            if (PrimitiveVertexData.vertexAttributes == null)
                PrimitiveVertexData.setVertexAttributes();
            return PrimitiveVertexData.vertexAttributes;
        }

        public int Size()
        {
            return 16;
        }

        public override string ToString()
        {
            return this.position.ToString() + ",\t" + this.color.ToString();
        }
    }
}
