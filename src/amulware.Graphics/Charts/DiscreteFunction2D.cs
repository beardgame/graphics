using System;
using System.Collections.Generic;
using System.Linq;

namespace amulware.Graphics.Charts
{
    abstract public class DiscreteFunction2D<TPoint>
        where TPoint : IPoint2D
    {
        private readonly List<TPoint> points = new List<TPoint>();

        private double minDrawX;
        private double maxDrawX;

        private int minDrawnIndex;
        private int maxDrawnIndex;


        public DiscreteFunction2D(IEnumerable<TPoint> points)
        {
            this.points = points.OrderBy(p => p.Position.X).ToList();
            this.MinDrawX = double.NegativeInfinity;
            this.MaxDrawX = double.PositiveInfinity;
        }

        public double MinDrawX
        {
            get { return this.minDrawX; }
            set
            {
                this.minDrawX = value;
                this.updateMinBound();
            }
        }

        public double MaxDrawX
        {
            get { return this.maxDrawX; }
            set
            {
                this.maxDrawX = value;
                this.updateMaxBound();
            }
        }

        private void updateMinBound()
        {
            var i = this.points.FindIndex(p => p.Position.X >= this.minDrawX);
            this.minDrawnIndex = i == -1 ? int.MaxValue : i;
        }

        private void updateMaxBound()
        {
            var i = this.points.FindLastIndex(p => p.Position.X <= this.maxDrawX);
            this.maxDrawnIndex = i == -1 ? Int16.MinValue : i;
        }

        protected IEnumerable<TPoint> drawablePoints
        {
            get
            {
                for (int i = this.minDrawnIndex; i <= this.maxDrawnIndex; i++)
                {
                    yield return this.points[i];
                }
            }
        } 
    }
}