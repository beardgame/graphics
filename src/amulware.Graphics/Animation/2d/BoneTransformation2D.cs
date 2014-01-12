using OpenTK;

namespace amulware.Graphics.Animation
{
    public class BoneTransformation2D : IBoneTransformation<BoneParameters2D, BoneTransformation2D>
    {

        private ITransformedBone<BoneParameters2D, BoneTransformation2D> parent;

        private BoneParameters2D parameters;

        private bool localAngleChanged = true;

        private float angleGlobal;
        private Matrix2 rotationLocal;
        private Matrix2 rotationGlobal;
        private Vector2 offsetGlobal;
        private float scaleGlobal;


        public float AngleLocal { get { return this.parameters.Angle; } }
        public float ScaleLocal { get { return this.parameters.Scale; } }

        public Vector2 OffsetLocal { get { return this.parameters.Offset; } }


        public float AngleGlobal { get { return this.angleGlobal; } }
        public Matrix2 RotationGlobal { get { return this.rotationGlobal; } }
        public Matrix2 RotationLocal { get { return this.rotationLocal; } }
        public Vector2 OffsetGlobal { get { return this.offsetGlobal; } }
        public float ScaleGlobal { get { return this.scaleGlobal; } }


        public void Recalculate()
        {
            if (this.localAngleChanged)
            {
                this.rotationLocal = Matrix2.CreateRotation(this.parameters.Angle) * this.parameters.Scale;
                this.localAngleChanged = false;
            }

            if (this.parent == null)
            {
                this.angleGlobal = this.parameters.Angle;
                this.offsetGlobal = this.parameters.Offset;
                this.rotationGlobal = this.rotationLocal;
                this.scaleGlobal = this.parameters.Scale;
            }
            else
            {
                var t = parent.Transformation;
                this.angleGlobal = t.angleGlobal + this.parameters.Angle;
                this.offsetGlobal = t.offsetGlobal + t.rotationGlobal * this.parameters.Offset;
                this.rotationGlobal = t.rotationGlobal * this.rotationLocal;
                this.scaleGlobal = t.ScaleGlobal * this.parameters.Scale;
            }
        }

        public void SetParent(ITransformedBone<BoneParameters2D, BoneTransformation2D> parent)
        {
            this.parent = parent;
        }

        public void SetParameters(BoneParameters2D parameters)
        {
            if (parameters.Angle != this.parameters.Angle || parameters.Scale != this.parameters.Scale)
                this.localAngleChanged = true;
        }
    }
}
