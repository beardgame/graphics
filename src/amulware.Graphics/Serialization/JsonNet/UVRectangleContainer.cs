using OpenToolkit.Mathematics;

namespace amulware.Graphics.Serialization.JsonNet
{
    /// <summary>
    /// Container used for Deserialisation of <see cref="UVRectangle"/>
    /// </summary>
    public class UVRectangleContainer
    {
        private readonly UVRectangle uv;
        private readonly bool absolute;

        /// <summary>
        /// Create a new <see cref="UVRectangleContainer"/>.
        /// </summary>
        /// <param name="uv">The <see cref="UVRectangle"/> to box</param>
        /// <param name="absolite">Whether the boxed <see cref="UVRectangle"/> has relative of absolute ub coordinates</param>
        public UVRectangleContainer(UVRectangle uv, bool absolute)
        {
            this.uv = uv;
            this.absolute = absolute;
        }

        /// <summary>
        /// Retuns the boxed <see cref="UVRectangle"/>.
        /// </summary>
        public UVRectangle GetUVRectangle()
        {
            return this.uv;
        }

        /// <summary>
        /// Retuns the boxed <see cref="UVRectangle"/> after applying the given scalar, if the uv rectangle is marked as having absolute positions.
        /// </summary>
        /// <param name="uvSize">The absolute size of the texture this <see cref="UVRectangle"/> is a part of</param>
        public UVRectangle GetUVRectangle(Vector2 uvSize)
        {
            if (!this.absolute)
                return this.uv;
            UVRectangle uv = this.uv;
            Vector2 scale = new Vector2(1f / uvSize.X, 1f / uvSize.Y);
            uv.ReScale(scale);
            return uv;
        }
    }
}
