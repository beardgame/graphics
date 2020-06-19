using System.Collections.Generic;
using System.Linq;
using OpenToolkit.Mathematics;

namespace amulware.Graphics
{
    public static class Extensions
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

        public static Vector2 Times(this Matrix2 matrix, Vector2 vector) =>
            new Vector2(
                matrix.M11 * vector.X + matrix.M12 * vector.Y,
                matrix.M21 * vector.X + matrix.M22 * vector.Y
            );

        public static Vector3 Times(this Matrix3 matrix, Vector3 vector) =>
            new Vector3(
                matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z,
                matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z,
                matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z
            );

        public static Matrix3 ScaleBy(this Matrix3 matrix, float scale) =>
            new Matrix3(
                matrix.M11 * scale, matrix.M12 * scale, matrix.M13 * scale,
                matrix.M21 * scale, matrix.M22 * scale, matrix.M23 * scale,
                matrix.M31 * scale, matrix.M32 * scale, matrix.M33 * scale
            );
    }
}
