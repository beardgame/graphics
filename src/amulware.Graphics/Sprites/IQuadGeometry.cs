using OpenTK;
using System;

namespace amulware.Graphics
{
    public interface IQuadGeometry
    {
        float LineWidth { get; set; }
        Vector2 Size { get; set; }

        void DrawSprite(Vector2 position);
        void DrawSprite(Vector2 position, float angle);
        void DrawSprite(Vector2 position, float angle, float scale);
        void DrawSprite(Vector3 position);
        void DrawSprite(Vector3 position, float angle);
        void DrawSprite(Vector3 position, float angle, float scale);

        void DrawRectangle(Vector2 position);
        void DrawRectangle(Vector2 position, Vector2 size);
        void DrawRectangle(Vector3 position);
        void DrawRectangle(Vector3 position, Vector2 size);
        void DrawRectangle(float x, float y);
        void DrawRectangle(float x, float y, float w, float h);
        void DrawRectangle(float x, float y, float z, float w, float h);

        void DrawLine(Vector2 xy1, Vector2 xy2);
        void DrawLine(Vector3 xyz1, Vector3 xyz2);
        void DrawLine(float x1, float y1, float x2, float y2);
        void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2);
    }
}
