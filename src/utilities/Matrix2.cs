using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Struct representing a two dimensional matrix of <see cref="float"/>s.
    /// </summary>
    public struct Matrix2
    {
        /// <summary>Element 1,1 of the matrix</summary>
        public float M11;
        /// <summary>Element 2,1 of the matrix</summary>
        public float M21;
        /// <summary>Element 1,2 of the matrix</summary>
        public float M12;
        /// <summary>Element 2,2 of the matrix</summary>
        public float M22;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2"/> struct.
        /// </summary>
        /// <param name="m11">The M11.</param>
        /// <param name="m21">The M21.</param>
        /// <param name="m12">The M12.</param>
        /// <param name="m22">The M22.</param>
        public Matrix2(float m11, float m21, float m12, float m22)
        {
            this.M11 = m11;
            this.M21 = m21;
            this.M12 = m12;
            this.M22 = m22;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2"/> struct.
        /// </summary>
        /// <param name="row1">First row.</param>
        /// <param name="row2">Second row.</param>
        public Matrix2(Vector2 row1, Vector2 row2)
        {
            this.M11 = row1.X;
            this.M21 = row1.Y;
            this.M12 = row2.X;
            this.M22 = row2.Y;
        }

        /// <summary>
        /// Determinant of the matrix.
        /// </summary>
        public float Determinant
        {
            get
            {
                return this.M11 * this.M22 - this.M12 * this.M21;
            }
        }

        /// <summary>
        /// Inverse of the matrix.
        /// </summary>
        /// <exception cref="System.DivideByZeroException">Throws an exception of matrix is not invertible(determinant = 0).</exception>
        public Matrix2 Inverse
        {
            get
            {
                float det = this.Determinant;
                if (det == 0)
                {
                    throw new DivideByZeroException("Matrix is not invertible, determinant is zero!");
                }
                return new Matrix2(this.M22, -this.M21, -this.M12, this.M11) * (1.0f / det);
            }
        }

        /// <summary>
        /// The 2x2 identity matrix.
        /// </summary>
        public static Matrix2 Identity = new Matrix2(1, 0, 0, 1);

        /// <summary>
        /// Creates a two dimentional rotational matrix.
        /// </summary>
        /// <param name="angle">The angle to rotate, in radians.</param>
        /// <returns>Rotational matrix.</returns>
        public static Matrix2 CreateRotation(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix2(cos, sin, -sin, cos);
        }

        /// <summary>
        /// Inverts the matrix.
        /// </summary>
        public void Invert()
        {
            this = this.Inverse;
        }

        /// <summary>
        /// Multiplies the matrix with a scalar
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>New matrix</returns>
        public static Matrix2 operator *(float scalar, Matrix2 matrix)
        {
            return new Matrix2(matrix.M11 * scalar, matrix.M21 * scalar, matrix.M12 * scalar, matrix.M22 * scalar);
        }

        /// <summary>
        /// Multiplies the matrix with a scalar
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>New matrix</returns>
        public static Matrix2 operator *(Matrix2 matrix, float scalar)
        {
            return new Matrix2(matrix.M11 * scalar, matrix.M21 * scalar, matrix.M12 * scalar, matrix.M22 * scalar);
        }

        /// <summary>
        /// Transforms a <see cref="Vector2"/> with the matrix
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>Transformed <see cref="Vector2"/></returns>
        public static Vector2 operator *(Matrix2 matrix, Vector2 vector)
        {
            return new Vector2(matrix.M11 * vector.X + matrix.M12 * vector.Y, matrix.M21 * vector.X + matrix.M22 * vector.Y);
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>New matrix</returns>
        public static Matrix2 operator *(Matrix2 matrix1, Matrix2 matrix2)
        {
            return new Matrix2(
                matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21,
                matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21,
                matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22,
                matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22
                );
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>New matrix</returns>
        public static Matrix2 operator +(Matrix2 matrix1, Matrix2 matrix2)
        {
            return new Matrix2(matrix1.M11 + matrix2.M11, matrix1.M21 + matrix2.M21, matrix1.M12 + matrix2.M12, matrix1.M22 + matrix2.M22);
        }

    }
}
