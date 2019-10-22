using OpenTK;

namespace amulware.Graphics
{
    public struct FastChart2DBarVertex : IVertexData
    {
        private readonly Vector2 position;
        private readonly Vector2 size;
        private readonly Color color;

        private static VertexAttribute[] makeAttributes()
        {
            return VertexData.MakeAttributeArray(
                VertexData.MakeAttributeTemplate<Vector2>("v_position"),
                VertexData.MakeAttributeTemplate<Vector2>("v_size"),
                VertexData.MakeAttributeTemplate<Color>("v_color")
                );
        }
        
        private static VertexAttribute[] attributes;

        public FastChart2DBarVertex(Vector2 position, Vector2 size, Color color)
        {
            this.position = position;
            this.size = size;
            this.color = color;
        }

        public VertexAttribute[] VertexAttributes()
        {
            return attributes ?? (attributes = makeAttributes());
        }
    }
}
