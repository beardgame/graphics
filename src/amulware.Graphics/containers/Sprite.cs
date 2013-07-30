using OpenTK;
using System;
using System.Linq;
using System.Collections.Generic;

namespace amulware.Graphics
{
    public abstract class Sprite
    {
        private readonly string name;

        public string Name { get { return this.name; } }

        private readonly IQuadGeometry geometry;

        public IQuadGeometry Geometry { get { return this.geometry; } }

        public Sprite(string name, IQuadGeometry geometry)
        {
            this.name = name;
            this.geometry = geometry;
        }

        abstract public int AnimationFrame { get; set; }
        abstract public float AnimationTime { set; }

        #region Draw /// @name Draw

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by.</param>
        public void Draw(Vector2 position, float angle = 0, float scale = 1)
        {
            this.geometry.DrawSprite(position, angle, scale);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by.</param>
        public void Draw(Vector3 position, float angle = 0, float scale = 1)
        {
            this.geometry.DrawSprite(position, angle, scale);
        }

        #endregion

        #region Draw Animated /// @name Draw Animated

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by.</param>
        /// <param name="frame">Frame of the animation</param>
        public void Draw(Vector2 position, float angle = 0, float scale = 1, int frame = 0)
        {
            this.AnimationFrame = frame;
            this.geometry.DrawSprite(position, angle, scale);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by.</param>
        /// <param name="time">Time since begin of the animation</param>
        public void Draw(Vector2 position, float angle = 0, float scale = 1, float time = 0)
        {
            this.AnimationTime = time;
            this.geometry.DrawSprite(position, angle, scale);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by.</param>
        /// <param name="frame">Frame of the animation</param>
        public void Draw(Vector3 position, float angle = 0, float scale = 1, int frame = 0)
        {
            this.AnimationFrame = frame;
            this.geometry.DrawSprite(position, angle, scale);
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        /// <param name="angle">The angle to rotate the sprite by, around the z axis, in radians.</param>
        /// <param name="scale">An additional scalar to scale the sprite by.</param>
        /// <param name="time">Time since begin of the animation</param>
        public void Draw(Vector3 position, float angle = 0, float scale = 1, float time = 0)
        {
            this.AnimationTime = time;
            this.geometry.DrawSprite(position, angle, scale);
        }

        #endregion
    }

    public class Sprite<TVertexData> : Sprite
        where TVertexData : struct, IVertexData
    {
        new private readonly UVQuadGeometry<TVertexData> geometry;

        new public UVQuadGeometry<TVertexData> Geometry { get { return this.geometry; } }

        public float Duration { get; set; }

        private UVRectangle[] uvFrames;

        private int animationFrame = 0;
        override public int AnimationFrame
        {
            get
            {
                return this.animationFrame;
            }
            set
            {
                this.animationFrame = value % this.uvFrames.Length;
                this.geometry.UV = this.uvFrames[this.animationFrame];
            }
        }

        override public float AnimationTime
        {
            set 
            {
                this.AnimationFrame = (int)(value / this.Duration * this.uvFrames.Length);
            }
        }

        public Sprite(string name, IEnumerable<UVRectangle> frames, float duration, UVQuadGeometry<TVertexData> geometry)
            : base(name, geometry)
        {
            if (frames == null || (this.uvFrames = frames.ToArray<UVRectangle>()).Length == 0)
                this.uvFrames = new UVRectangle[] { UVRectangle.Default };
            this.Duration = duration;
            this.geometry = geometry;
        }

    }
}
