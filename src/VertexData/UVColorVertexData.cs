using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public struct UVColorVertexData : IVertexData
    {
        // add attributes and constructors here
        public Vector3 Position; // 12 bytes
        public Vector2 TexCoord; // 8 bytes
        public Color Color; // 4 bytes

        static private VertexAttribute[] vertexAttributes;

        public UVColorVertexData(Vector3 position, Vector2 uv, Color color)
        {
            this.Position = position;
            this.TexCoord = uv;
            this.Color = color;
        }

        public UVColorVertexData(float x, float y, float z, Vector2 uv, Color color)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Position.Z = z;
            this.TexCoord = uv;
            this.Color = color;
        }

        public UVColorVertexData(float x, float y, float z, float u, float v, Color color)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Position.Z = z;
            this.TexCoord.X = u;
            this.TexCoord.Y = v;
            this.Color = color;
        }

        static private void setVertexAttributes()
        {
            UVColorVertexData.vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, 24, 0),
                new VertexAttribute("v_texcoord", 2, VertexAttribPointerType.Float, 24, 12),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, 24, 20, true)
            };
        }

        public VertexAttribute[] VertexAttributes()
        {
            if (UVColorVertexData.vertexAttributes == null)
                UVColorVertexData.setVertexAttributes();
            return UVColorVertexData.vertexAttributes;
        }

        public int Size()
        {
            return 24;
        }
    }
}
