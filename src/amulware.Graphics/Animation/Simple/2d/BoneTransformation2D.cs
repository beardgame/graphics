using OpenTK;

namespace amulware.Graphics.Animation
{
    public class BoneTransformation2D<TBoneAttributes>
        : IBoneTransformation<BoneParameters2D, BoneParameters2D, TBoneAttributes, BoneTransformation2D<TBoneAttributes>>
    {
        private Bone<BoneParameters2D, BoneParameters2D, TBoneAttributes, BoneTransformation2D<TBoneAttributes>>
            parent;

        private BoneParameters2D parameters;

        private bool localAngleChanged;

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
                this.offsetGlobal = t.offsetGlobal + t.rotationGlobal.Times(this.parameters.Offset);
                this.rotationGlobal = t.rotationGlobal * this.rotationLocal;
                this.scaleGlobal = t.ScaleGlobal * this.parameters.Scale;
            }
        }

        public void SetBone(
            Bone<BoneParameters2D, BoneParameters2D, TBoneAttributes, BoneTransformation2D<TBoneAttributes>>
            bone)
        {
            this.parent = bone.Parent;
        }

        public void UpdateParameters(BoneParameters2D parameters)
        {
            if (parameters.Angle != this.parameters.Angle || parameters.Scale != this.parameters.Scale)
                this.localAngleChanged = true;
            this.parameters = parameters;
            this.Recalculate();
        }
    }
}
