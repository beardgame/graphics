using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class PostProcessSurface : StaticVertexSurface<PostProcessVertexData>
    {
        public PostProcessSurface()
            : base(BeginMode.Quads)
        {
            initVertices();
        }

        private void initVertices()
        {
            this.vertices = new PostProcessVertexData[4];
            this.vertexCount = 4;
            SetRectangle(-1, -1, 1, 1);
        }

        public void SetRectangle(Vector2 from, Vector2 to)
        {
            this.SetRectangle(from.X, from.Y, to.X, to.Y);
        }

        public void SetRectangle(float fromX, float fromY, float toX, float toY)
        {
            this.vertices[0] = new PostProcessVertexData(fromX, fromY);
            this.vertices[1] = new PostProcessVertexData(toX, fromY);
            this.vertices[2] = new PostProcessVertexData(toX, toY);
            this.vertices[3] = new PostProcessVertexData(fromX, toY);

        }
    }
}
