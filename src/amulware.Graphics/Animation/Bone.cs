using System;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace amulware.Graphics.Animation
{
    sealed public class Bone
    {
        private Bone parent;
        public Bone Parent { get { return this.parent; } }

        // parameters
        private BoneParameters parameters;

        // transformations
        private float angleGlobal;
        private Matrix2 rotationLocal;
        private Matrix2 rotationGlobal;
        private Vector2 offsetGlobal;
        private float scaleGlobal;

        private bool localAngleChanged = true;

        private readonly string sprite;
        public string Sprite { get { return this.sprite; } }

        public Bone(Bone parent, string sprite = null)
        {
            this.parent = parent;
            this.sprite = sprite;
        }

        public void SetParameters(BoneParameters parameters)
        {
            if (parameters.Angle != this.parameters.Angle || parameters.Scale != this.parameters.Scale)
                this.localAngleChanged = true;
            this.parameters = parameters;
        }

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
                this.angleGlobal = this.parent.angleGlobal + this.parameters.Angle;
                this.offsetGlobal = this.parent.offsetGlobal + this.parent.rotationGlobal * this.parameters.Offset;
                this.rotationGlobal = this.parent.rotationGlobal * this.rotationLocal;
                this.scaleGlobal = this.parent.ScaleGlobal * this.parameters.Scale;
            }
        }
    }
}
