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

        abstract public int AnimationFrameCount { get; }

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

    }

    public class Sprite<TVertexData> : Sprite
        where TVertexData : struct, IVertexData
    {
        private readonly UVQuadGeometry<TVertexData> geometry;

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

        public override int AnimationFrameCount
        {
            get { return this.uvFrames.Length; }
        }

        public Sprite(string name, IEnumerable<UVRectangle> frames, float duration,
            UVQuadGeometry<TVertexData> geometry, Vector2 size, bool setFirstFrame = true)
            : base(name, geometry)
        {
            if (frames == null || (this.uvFrames = frames.ToArray<UVRectangle>()).Length == 0)
                this.uvFrames = new UVRectangle[] { UVRectangle.Default };
            this.Duration = duration;
            this.geometry = geometry;
            this.geometry.Size = size;
            if (setFirstFrame)
                this.AnimationFrame = 0;
        }

        public Sprite(Sprite<TVertexData> template,
            UVQuadGeometry<TVertexData> geometry, bool setFirstFrame = true)
            : base(template.Name, geometry)
        {
            this.uvFrames = template.uvFrames;
            this.Duration = template.Duration;
            this.geometry = geometry;
            this.geometry.Size = template.geometry.Size;
            if (setFirstFrame)
                this.AnimationFrame = 0;
        }

        private Sprite(string name, UVRectangle[] frames, float duration,
            UVQuadGeometry<TVertexData> geometry, Vector2 size, bool setFirstFrame = true)
            : base(name, geometry)
        {
            this.uvFrames = frames;
            this.Duration = duration;
            this.geometry = geometry;
            this.geometry.Size = size;
            if (setFirstFrame)
                this.AnimationFrame = 0;
        }

        public static Sprite<TVertexData> Copy<TVertexDataIn>
            (Sprite<TVertexDataIn> template, UVQuadGeometry<TVertexData> geometry, bool setFirstFrame = true)
            where TVertexDataIn : struct, IVertexData
        {
            return new Sprite<TVertexData>(template.Name, template.uvFrames, template.Duration,
                geometry, template.geometry.Size, setFirstFrame);
        }

    }
}
