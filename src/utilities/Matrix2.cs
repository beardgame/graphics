using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public struct Matrix2
    {
        public float M11;
        public float M21;
        public float M12;
        public float M22;

        public Matrix2(float m11, float m21, float m12, float m22)
        {
            this.M11 = m11;
            this.M21 = m21;
            this.M12 = m12;
            this.M22 = m22;
        }

        public Matrix2(Vector2 row1, Vector2 row2)
        {
            this.M11 = row1.X;
            this.M21 = row1.Y;
            this.M12 = row2.X;
            this.M22 = row2.Y;
        }

        public float Determinant
        {
            get
            {
                return this.M11 * this.M22 - this.M12 * this.M21;
            }
        }

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

        public static Matrix2 Identity = new Matrix2(1, 0, 0, 1);

        public static Matrix2 CreateRotation(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix2(cos, sin, -sin, cos);
        }

        public void Invert()
        {
            this = this.Inverse;
        }

        public static Matrix2 operator *(float scalar, Matrix2 matrix)
        {
            return new Matrix2(matrix.M11 * scalar, matrix.M21 * scalar, matrix.M12 * scalar, matrix.M22 * scalar);
        }

        public static Matrix2 operator *(Matrix2 matrix, float scalar)
        {
            return new Matrix2(matrix.M11 * scalar, matrix.M21 * scalar, matrix.M12 * scalar, matrix.M22 * scalar);
        }

        public static Vector2 operator *(Matrix2 matrix, Vector2 vector)
        {
            return new Vector2(matrix.M11 * vector.X + matrix.M12 * vector.Y, matrix.M21 * vector.X + matrix.M22 * vector.Y);
        }

        public static Matrix2 operator *(Matrix2 matrix1, Matrix2 matrix2)
        {
            return new Matrix2(
                matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21,
                matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21,
                matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22,
                matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22
                );
        }

        public static Matrix2 operator +(Matrix2 matrix1, Matrix2 matrix2)
        {
            return new Matrix2(matrix1.M11 + matrix2.M11, matrix1.M21 + matrix2.M21, matrix1.M12 + matrix2.M12, matrix1.M22 + matrix2.M22);
        }

    }
}
