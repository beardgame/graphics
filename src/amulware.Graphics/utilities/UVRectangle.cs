using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This struct represents a set of UV coordinates used to map textures onto quads.
    /// </summary>
    public struct UVRectangle
    {
        /// <summary>
        /// UV coordinates of top left vertex
        /// </summary>
        public Vector2 TopLeft;
        /// <summary>
        /// UV coordinates of top right vertex
        /// </summary>
        public Vector2 TopRight;
        /// <summary>
        /// UV coordinates of bottom left vertex
        /// </summary>
        public Vector2 BottomLeft;
        /// <summary>
        /// UV coordinates of bottom right vertex
        /// </summary>
        public Vector2 BottomRight;

        /// <summary>
        /// Constructs a <see cref="UVRectangle"/> given the coordinates of its four vertices.
        /// </summary>
        /// <param name="topLeft">UV coordinates of top left vertex</param>
        /// <param name="topRight">UV coordinates of top right vertex</param>
        /// <param name="bottomLeft">UV coordinates of bottom left vertex</param>
        /// <param name="bottomRight">UV coordinates of bottom right vertex</param>
        public UVRectangle(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
        {
            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
            this.BottomRight = bottomRight;
        }

        /// <summary>
        /// Constructs an axis aligned <see cref="UVRectangle"/> given the left, top, right and bottom boundary.
        /// </summary>
        /// <param name="left">Left boundary</param>
        /// <param name="right">Right boundary</param>
        /// <param name="top">Top boundary</param>
        /// <param name="bottom">Bottom Boundary</param>
        public UVRectangle(float left, float right, float top, float bottom)
        {
            this.TopLeft.X = left;
            this.TopLeft.Y = top;
            this.TopRight.X = right;
            this.TopRight.Y = top;
            this.BottomLeft.X = left;
            this.BottomLeft.Y = bottom;
            this.BottomRight.X = right;
            this.BottomRight.Y = bottom;
        }

        /// <summary>
        /// Rescales th UVRectangle by multiplying all coordinates with the given vector component-wise.
        /// </summary>
        /// <param name="scale">Scalar vector</param>
        public void ReScale(Vector2 scale)
        {
            this.TopLeft.X *= scale.X;
            this.TopLeft.Y *= scale.Y;
            this.TopRight.X *= scale.X;
            this.TopRight.Y *= scale.Y;
            this.BottomLeft.X *= scale.X;
            this.BottomLeft.Y *= scale.Y;
            this.BottomRight.X *= scale.X;
            this.BottomRight.Y *= scale.Y;
        }

        /// <summary>
        /// Rescales the UVRectangle by multiplying all coordinates with the given scalar.
        /// </summary>
        /// <param name="scale">The scalar</param>
        public void ReScale(float scale)
        {
            this.TopLeft.X *= scale;
            this.TopLeft.Y *= scale;
            this.TopRight.X *= scale;
            this.TopRight.Y *= scale;
            this.BottomLeft.X *= scale;
            this.BottomLeft.Y *= scale;
            this.BottomRight.X *= scale;
            this.BottomRight.Y *= scale;
        }

        /// <summary>
        /// The center of the <see cref="UVRectangle"/>(arithmetic mean of the four corners)
        /// </summary>
        /// <remarks>
        /// This property is recalculated for every call.
        /// </remarks>
        public Vector2 Center { get { return 0.25f * (this.BottomLeft + this.BottomRight + this.TopLeft + this.TopRight); } }

        /// <summary>
        /// Rotates the four corners of the <see cref="UVRectangle"/> by a given angle around a given point.
        /// </summary>
        /// <param name="angle">The angle by which to rotate</param>
        /// <param name="center">The point around which to rotate</param>
        public UVRectangle Rotated(float angle, Vector2 center)
        {
            Matrix2 m = Matrix2.CreateRotation(angle);
            return new UVRectangle(
                center + m * (this.TopLeft - center),
                center + m * (this.TopRight - center),
                center + m * (this.BottomLeft - center),
                center + m * (this.BottomRight - center)
                );
        }

        /// <summary>
        /// Rotates the four corners of the <see cref="UVRectangle"/> by a given angle around its <see cref="Center"/>.
        /// </summary>
        /// <param name="angle">The angle by which to rotate</param>
        public UVRectangle Rotated(float angle)
        {
            return Rotated(angle, this.Center);
        }

        /// <summary>
        /// Flips the <see cref="UVRectangle"/> horizontally.
        /// </summary>
        public UVRectangle FlippedH()
        {
            return new UVRectangle(
                new Vector2(this.TopRight.X, this.TopLeft.Y),
                new Vector2(this.TopLeft.X, this.TopRight.Y),
                new Vector2(this.BottomRight.X, this.BottomLeft.Y),
                new Vector2(this.BottomLeft.X, this.BottomRight.Y)
                );
        }

        /// <summary>
        /// Flips the <see cref="UVRectangle"/> vertically.
        /// </summary>
        public UVRectangle FlippedV()
        {
            return new UVRectangle(
                new Vector2(this.TopLeft.X, this.BottomLeft.Y),
                new Vector2(this.TopRight.X, this.BottomRight.Y),
                new Vector2(this.BottomLeft.X, this.TopLeft.Y),
                new Vector2(this.BottomRight.X, this.TopRight.Y)
                );
        }

        /// <summary>
        /// Default <see cref="UVRectangle"/>, from (0, 0) to (1, 1)
        /// </summary>
        static public readonly UVRectangle Default = new UVRectangle(0, 1, 0, 1);
    }
}
