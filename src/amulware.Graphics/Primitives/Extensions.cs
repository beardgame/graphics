using OpenToolkit.Mathematics;

namespace amulware.Graphics
{
    internal static class Extensions
    {
        public static Vector2 Times(this Matrix2 matrix, Vector2 vector) =>
            new Vector2(
                matrix.M11 * vector.X + matrix.M12 * vector.Y,
                matrix.M21 * vector.X + matrix.M22 * vector.Y
            );
    }
}
