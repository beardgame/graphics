using System;
using System.Collections.Generic;
using System.Linq;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Charts
{
    public abstract class RegularDiscreteFunction2D<TValue> : IChart2DComponent
    {
        private readonly List<TValue> values;

        private int maxNumberOfValues;

        private double drawStepSize = 1f;
        private double drawOffset;

        protected RegularDiscreteFunction2D()
            : this(Enumerable.Empty<TValue>())
        {

        }

        protected RegularDiscreteFunction2D(IEnumerable<TValue> values)
        {
            this.values = values.ToList();
            this.maxNumberOfValues = this.values.Count;
        }

        public int MaxNumberOfValues
        {
            get { return this.maxNumberOfValues; }
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Maximum number of values must be positive.");
                this.maxNumberOfValues = value;
                this.truncateIfNeeded();
            }
        }

        protected void truncateIfNeeded()
        {
            if (this.values.Count > 16 &&
                this.values.Count > 2 * this.maxNumberOfValues)
                this.Truncate();
        }

        public void Truncate(bool freeUnusedMemory = false)
        {
            if (this.values.Count <= this.maxNumberOfValues)
                return;
            this.values.RemoveRange(0, this.values.Count - this.maxNumberOfValues);
            if(freeUnusedMemory)
                this.values.TrimExcess();
        }

        public void Add(TValue value)
        {
            this.values.Add(value);
            this.truncateIfNeeded();
        }

        public double DrawStepSize { get { return this.drawStepSize; } set { this.drawStepSize = value; } }
        public double DrawOffset { get { return this.drawOffset; } set { this.drawOffset = value; } }

        protected IEnumerable<DrawableValue> drawnPoints(IAxis axis1, Vector2 offset)
        {
            int minI = Math.Max(0, this.values.Count - this.maxNumberOfValues);
            int xI = 0;
            for (int i = minI; i < this.values.Count; i++)
            {
                var x = (float)(axis1.DataToChart(xI * this.drawStepSize) + this.drawOffset) + offset.X;
                var v = this.values[i];

                yield return new DrawableValue(v, x);

                xI++;
            }
        }

        protected struct DrawableValue
        {
            private readonly TValue value;
            private readonly float xCoordinate;

            public DrawableValue(TValue value, float xCoordinate) : this()
            {
                this.value = value;
                this.xCoordinate = xCoordinate;
            }

            public float XCoordinate
            {
                get { return this.xCoordinate; }
            }

            public TValue Value
            {
                get { return this.value; }
            }
        }

        public abstract void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset);
    }

    public class RegularDiscreteFunction2D : RegularDiscreteFunction2D<double>
    {
        private readonly Color color;

        public RegularDiscreteFunction2D(Color color)
            : this(Enumerable.Empty<double>(), color)
        {
        }

        public RegularDiscreteFunction2D(IEnumerable<double> values, Color color)
            : base(values)
        {
            this.color = color;
            this.AreaTransparency = 0.5f;
        }

        public DrawMode DrawMode { get; set; }
        public float AreaTransparency { get; set; }

        private IEnumerable<Vector2> drawnPoints(IAxis axis1, IAxis axis2, Vector2 offset)
        {
            return this.drawnPoints(axis1, offset).Select(v =>
                new Vector2(v.XCoordinate, (float)axis2.DataToChart(v.Value) + offset.Y));
        }

        public override void Draw(Chart2DSpriteContainer sprites, IAxis axis1, IAxis axis2, Vector2 offset)
        {
            Vector2 previous = default(Vector2);
            bool first = true;

            var areaColor = this.color * this.AreaTransparency;

            float baseY = (float)axis2.DataToChart(0) + offset.Y;

            sprites.Color = this.color;
            switch (this.DrawMode)
            {
                case DrawMode.Points:
                    foreach (var point in this.drawnPoints(axis1, axis2, offset))
                    {
                        sprites.DrawPoint(point, sprites.LargePointSize);
                    }
                    break;
                case DrawMode.Line:
                    foreach (var point in this.drawnPoints(axis1, axis2, offset))
                    {
                        if (first)
                        {
                            previous = point;
                            first = false;
                            continue;
                        }
                        sprites.DrawLine(previous, point, sprites.ThickLineWidth);
                        previous = point;
                    }
                    break;
                case DrawMode.Area:
                    foreach (var point in this.drawnPoints(axis1, axis2, offset))
                    {
                        if (first)
                        {
                            previous = point;
                            first = false;
                            continue;
                        }
                        sprites.Color = areaColor;
                        sprites.DrawQuad(new Vector2(previous.X, baseY), previous, point, new Vector2(point.X, baseY));
                        sprites.Color = this.color;
                        sprites.DrawLine(previous, point, sprites.ThinLineWidth);
                        previous = point;
                    }
                    break;
                case DrawMode.Bar:
                    foreach (var point in this.drawnPoints(axis1, axis2, offset))
                    {
                        if (first)
                        {
                            previous = point;
                            first = false;
                            continue;
                        }
                        previous.Y = point.Y;
                        sprites.Color = areaColor;
                        sprites.DrawQuad(new Vector2(previous.X, baseY), previous, point, new Vector2(point.X, baseY));
                        sprites.Color = this.color;
                        sprites.DrawLine(previous, point, sprites.ThinLineWidth);
                        previous = point;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum DrawMode
    {
        Points = 0,
        Line = 1,
        Area = 2,
        Bar = 3,
    }
}
