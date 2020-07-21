using System;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace amulware.Graphics
{
    public sealed class MeshBuilder<TVertex> : IIndexedMeshBuilder<TVertex, ushort>
        where TVertex : struct, IVertexData
    {
        private readonly MutableBuffer<TVertex> vertices;
        private readonly MutableBuffer<ushort> indices;

        public MeshBuilder()
        {
            vertices = new MutableBuffer<TVertex>();
            indices = new MutableBuffer<ushort>();
        }

        public void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<ushort> indices, out ushort indexOffset)
        {
            indexOffset = (ushort) this.vertices.Count;
            vertices = this.vertices.AddRange(vertexCount);
            indices = this.indices.AddRange(indexCount);
        }

        public IRenderable ToRenderable()
        {
            return Renderable.ForVerticesAndIndices(vertices, indices, PrimitiveType.Triangles);
        }
    }

    public interface IIndexedMeshBuilder<TVertex, TIndex>
    {
        void Add(
            int vertexCount, int indexCount,
            out Span<TVertex> vertices, out Span<TIndex> indices, out TIndex indexOffset);
    }

    public static class IndexedMeshBuilderExtensions
    {
        public static void AddTriangle<TVertex>(
            this IIndexedMeshBuilder<TVertex, ushort> meshBuilder, in TVertex v0, in TVertex v1, in TVertex v2)
        {
            meshBuilder.Add(3, 3, out var vertices, out var indices, out var indexOffset);

            vertices[0] = v0;
            vertices[1] = v1;
            vertices[2] = v2;

            indices[0] = indexOffset;
            indices[1] = (ushort) (indexOffset + 1);
            indices[2] = (ushort) (indexOffset + 2);
        }

        public static void AddQuad<TVertex>(
            this IIndexedMeshBuilder<TVertex, ushort> meshBuilder,
            in TVertex v0, in TVertex v1, in TVertex v2, in TVertex v3)
        {
            meshBuilder.Add(4, 6, out var vertices, out var indices, out var indexOffset);

            vertices[0] = v0;
            vertices[1] = v1;
            vertices[2] = v2;
            vertices[3] = v3;

            indices[0] = indexOffset;
            indices[1] = (ushort) (indexOffset + 1);
            indices[2] = (ushort) (indexOffset + 2);

            indices[3] = indexOffset;
            indices[4] = (ushort) (indexOffset + 2);
            indices[5] = (ushort) (indexOffset + 3);
        }
    }

    public sealed class ColorShapeDrawer
    {
        private readonly IIndexedMeshBuilder<ColorVertexData, ushort> meshBuilder;

        public ColorShapeDrawer(IIndexedMeshBuilder<ColorVertexData, ushort> meshBuilder)
        {
            this.meshBuilder = meshBuilder;
        }

        public void FillRectangle(Vector2 xy, Vector2 wh, Color color)
        {
            FillRectangle(xy.X, xy.Y, 0, wh.X, wh.Y, color);
        }

        public void FillRectangle(Vector3 xyz, Vector2 wh, Color color)
        {
            FillRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y, color);
        }

        public void FillRectangle(float x, float y, float w, float h, Color color)
        {
            FillRectangle(x, y, 0, w, h, color);
        }

        public void FillRectangle(float x, float y, float z, float w, float h, Color color)
        {
            meshBuilder.AddQuad(
                new ColorVertexData(x, y, z, color),
                new ColorVertexData(x + w, y, z, color),
                new ColorVertexData(x + w, y + h, z, color),
                new ColorVertexData(x, y + h, z, color)
            );
        }

        public void DrawRectangle(Vector2 xy, Vector2 wh, Color color, float lineWidth)
        {
            DrawRectangle(xy.X, xy.Y, 0, wh.X, wh.Y, color, lineWidth);
        }
        public void DrawRectangle(Vector3 xyz, Vector2 wh, Color color, float lineWidth)
        {
            DrawRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y, color, lineWidth);
        }
        public void DrawRectangle(float x, float y, float w, float h, Color color, float lineWidth)
        {
            DrawRectangle(x, y, 0, w, h, color, lineWidth);
        }

        public void DrawRectangle(float x, float y, float z, float w, float h, Color color, float lineWidth)
        {
            meshBuilder.Add(8, 24, out var vertices, out var indices, out var indexOffset);

            // outer
            vertices[0] = new ColorVertexData(x, y, z, color);
            vertices[1] = new ColorVertexData(x + w, y, z, color);
            vertices[2] = new ColorVertexData(x + w, y + h, z, color);
            vertices[3] = new ColorVertexData(x, y + h, z, color);

            // inner
            vertices[4] = new ColorVertexData(x + lineWidth, y + lineWidth, z, color);
            vertices[5] = new ColorVertexData(x + w - lineWidth, y + lineWidth, z, color);
            vertices[6] = new ColorVertexData(x + w - lineWidth, y + h - lineWidth, z, color);
            vertices[7] = new ColorVertexData(x + lineWidth, y + h - lineWidth, z, color);

            var indicesIndex = 0;
            for (var i = 0; i < 4; i++)
            {
                indices[indicesIndex++] = (ushort) (indexOffset + i);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 1) % 4);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 1) % 4 + 4);

                indices[indicesIndex++] = (ushort) (indexOffset + i);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 3) % 4 + 4);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 3) % 4);
            }
        }

        public void FillCircle(float x, float y, float r, Color color, int edges = 32)
        {
            fillOval(x, y, 0, r, r, color, edges);
        }

        public void FillCircle(Vector2 xy, float r, Color color, int edges = 32)
        {
            fillOval(xy.X, xy.Y, 0, r, r, color, edges);
        }

        public void FillCircle(float x, float y, float z, float r, Color color, int edges = 32)
        {
            fillOval(x, y, z, r, r, color, edges);
        }

        public void FillCircle(Vector3 xyz, float r, Color color, int edges = 32)
        {
            fillOval(xyz.X, xyz.Y, xyz.Z, r, r, color, edges);
        }

        public void DrawCircle(float x, float y, float r, Color color, float lineWidth, int edges = 32)
        {
            drawOval(x, y, 0, r, r, color, lineWidth, edges);
        }

        public void DrawCircle(Vector2 xy, float r, Color color, float lineWidth, int edges = 32)
        {
            drawOval(xy.X, xy.Y, 0, r, r, color, lineWidth, edges);
        }

        public void DrawCircle(float x, float y, float z, float r, Color color, float lineWidth, int edges = 32)
        {
            drawOval(x, y, z, r, r, color, lineWidth, edges);
        }

        public void DrawCircle(Vector3 xyz, float r, Color color, float lineWidth, int edges = 32)
        {
            drawOval(xyz.X, xyz.Y, xyz.Z, r, r, color, lineWidth, edges);
        }

        public void FillOval(Vector2 xy, Vector2 wh, Color color, int edges = 32)
        {
            wh *= 0.5f;
            fillOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, color, edges);
        }

        public void FillOval(Vector3 xyz, Vector2 wh, Color color, int edges = 32)
        {
            wh *= 0.5f;
            fillOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, color, edges);
        }

        public void FillOval(float x, float y, float w, float h, Color color, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            fillOval(x + w, y + h, 0, w, h, color, edges);
        }

        public void FillOval(float x, float y, float z, float w, float h, Color color, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            fillOval(x + w, y + h, z, w, h, color, edges);
        }

        public void DrawOval(Vector2 xy, Vector2 wh, Color color, float lineWidth, int edges = 32)
        {
            wh *= 0.5f;
            drawOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, color, lineWidth, edges);
        }

        public void DrawOval(Vector3 xyz, Vector2 wh, Color color, float lineWidth, int edges = 32)
        {
            wh *= 0.5f;
            drawOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, color, lineWidth, edges);
        }

        public void DrawOval(float x, float y, float w, float h, Color color, float lineWidth, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawOval(x + w, y + h, 0, w, h, color, lineWidth, edges);
        }

        public void DrawOval(float x, float y, float z, float w, float h, Color color, float lineWidth, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawOval(x + w, y + h, z, w, h, color, lineWidth, edges);
        }

        private void fillOval(
            float centerX, float centerY, float centerZ, float halfWidth, float halfHeight, Color color, int edges)
        {
            if (edges < 3)
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least three edges.");

            meshBuilder.Add(edges, (edges - 2) * 3, out var vertices, out var indices, out var indexOffset);

            var rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            var xy = new Vector2(0, -1);

            vertices[0] = new ColorVertexData(
                centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ, color);

            for (var i = 1; i < edges; i++)
            {
                xy = rotation.Times(xy);

                vertices[i] = new ColorVertexData(
                    centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ, color);
            }

            for (var i = 0; i < edges - 2; i++)
            {
                var o = i * 3;
                indices[o++] = indexOffset;
                indices[o++] = (ushort)(indexOffset + i + 1);
                indices[o] = (ushort)(indexOffset + i + 2);
            }
        }

        private void drawOval(
            float centerX, float centerY, float centerZ, float halfWidth, float halfHeight,
            Color color, float lineWidth, int edges)
        {
            if (edges < 3)
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least three edges.");

            meshBuilder.Add(edges * 2, edges * 6, out var vertices, out var indices, out var indexOffset);

            var rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            var xy = new Vector2(0, -1);

            var innerW = halfWidth - lineWidth;
            var innerH = halfHeight - lineWidth;

            vertices[0] = new ColorVertexData(
                centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ, color);

            vertices[1] = new ColorVertexData(
                centerX + xy.X * innerW, centerY + xy.Y * innerH, centerZ, color);

            for (var i = 1; i < edges; i++)
            {
                xy = rotation.Times(xy);

                vertices[2 * i] = new ColorVertexData(
                    centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ, color);

                vertices[2 * i + 1] = new ColorVertexData(
                    centerX + xy.X * innerW, centerY + xy.Y * innerH, centerZ, color);
            }

            for (var i = 0; i < edges - 1; i++)
            {
                var j = i * 6;
                var o = i * 2;
                indices[j] = (ushort)(indexOffset + o);
                indices[j + 1] = (ushort)(indexOffset + o + 2);
                indices[j + 2] = (ushort)(indexOffset + o + 1);

                indices[j + 3] = (ushort)(indexOffset + o + 1);
                indices[j + 4] = (ushort)(indexOffset + o + 2);
                indices[j + 5] = (ushort)(indexOffset + o + 3);
            }

            var lastJ = edges * 6 - 6;

            indices[lastJ] = (ushort)(indexOffset + edges * 2 - 2);
            indices[lastJ + 1] = indexOffset;
            indices[lastJ + 2] = (ushort)(indexOffset + edges * 2 - 1);

            indices[lastJ + 3] = (ushort)(indexOffset + edges * 2 - 1);
            indices[lastJ + 4] = indexOffset;
            indices[lastJ + 5] = (ushort)(indexOffset + 1);
        }

        public void DrawLine(Vector2 xy1, Vector2 xy2, Color color, float lineWidth)
        {
            DrawLine(xy1.X, xy1.Y, 0, xy2.X, xy2.Y, 0, color, lineWidth);
        }

        public void DrawLine(Vector3 xyz1, Vector3 xyz2, Color color, float lineWidth)
        {
            DrawLine(xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, color, lineWidth);
        }

        public void DrawLine(float x1, float y1, float x2, float y2, Color color, float lineWidth)
        {
            DrawLine(x1, y1, 0, x2, y2, 0, color, lineWidth);
        }

        public void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2, Color color, float lineWidth)
        {
            var vx = x2 - x1;
            var vy = y1 - y2; // switch order for correct normal direction
            var ilxy = lineWidth / (float)Math.Sqrt(vx * vx + vy * vy);
            var nx = vy * ilxy;
            var ny = vx * ilxy;
            meshBuilder.AddQuad(
                new ColorVertexData(x1 + nx, y1 + ny, z1, color),
                new ColorVertexData(x1 - nx, y1 - ny, z1, color),
                new ColorVertexData(x2 - nx, y2 - ny, z2, color),
                new ColorVertexData(x2 + nx, y2 + ny, z2, color)
                );
        }
    }
}
