using OpenTK;

namespace amulware.Graphics.Animation
{
    public class BoneTransformation3D<TBoneAttributes>
        : IBoneTransformation<BoneParameters3D, BoneParameters3D, TBoneAttributes, BoneTransformation3D<TBoneAttributes>>
    {
        private Bone<BoneParameters3D, BoneParameters3D, TBoneAttributes, BoneTransformation3D<TBoneAttributes>>
            parent;

        private BoneParameters3D parameters;

        private bool localAngleChanged;

        private Matrix3 rotationLocal;
        private Matrix3 rotationGlobal;
        private Vector3 offsetGlobal;
        private float scaleGlobal;

        public float ScaleLocal { get { return this.parameters.Scale; } }

        public Vector3 OffsetLocal { get { return this.parameters.Offset; } }

        public Matrix3 RotationGlobal { get { return this.rotationGlobal; } }
        public Matrix3 RotationLocal { get { return this.rotationLocal; } }
        public Vector3 OffsetGlobal { get { return this.offsetGlobal; } }
        public float ScaleGlobal { get { return this.scaleGlobal; } }


        public void Recalculate()
        {
            if (this.localAngleChanged)
            {
                this.rotationLocal =
                    (Matrix3.CreateRotationX(this.parameters.AngleX) *
                    Matrix3.CreateRotationY(this.parameters.AngleY) *
                    Matrix3.CreateRotationZ(this.parameters.AngleZ))
                    .ScaleBy(this.parameters.Scale);
                        
                this.localAngleChanged = false;
            }

            if (this.parent == null)
            {
                this.offsetGlobal = this.parameters.Offset;
                this.rotationGlobal = this.rotationLocal;
                this.scaleGlobal = this.parameters.Scale;
            }
            else
            {
                var t = parent.Transformation;
                this.offsetGlobal = t.offsetGlobal + t.rotationGlobal.Times(this.parameters.Offset);
                this.rotationGlobal = t.rotationGlobal * this.rotationLocal;
                this.scaleGlobal = t.ScaleGlobal * this.parameters.Scale;
            }
        }

        public void SetBone(
            Bone<BoneParameters3D, BoneParameters3D, TBoneAttributes, BoneTransformation3D<TBoneAttributes>>
            bone)
        {
            this.parent = bone.Parent;
        }

        public void UpdateParameters(BoneParameters3D parameters)
        {
            if (parameters.AngleX != this.parameters.AngleX
                || parameters.AngleY != this.parameters.AngleY
                || parameters.AngleZ != this.parameters.AngleZ
                || parameters.Scale != this.parameters.Scale)
                this.localAngleChanged = true;
            this.parameters = parameters;
            this.Recalculate();
        }
    }
}
