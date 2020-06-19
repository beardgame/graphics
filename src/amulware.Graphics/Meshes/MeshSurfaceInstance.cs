using OpenToolkit.Mathematics;

namespace amulware.Graphics.Meshes
{
    public class MeshSurfaceInstance
    {
        private readonly Surface surface;

        private readonly Matrix4Uniform transformUniform;

        private bool isTransformOutOfDate = true;
        private Matrix4 transform;

        private float scale = 1;
        private float pitch;
        private float yaw;
        private float roll;
        private Vector3 translation;

        public MeshSurfaceInstance(Surface surface, Matrix4Uniform transformUniform)
        {
            this.surface = surface;
            this.transformUniform = transformUniform;
        }

        #region public properties

        public float Scale
        {
            get { return this.scale; }
            set { this.scale = value; this.markDirty(); }
        }

        public float Pitch
        {
            get { return this.pitch; }
            set { this.pitch = value; this.markDirty(); }
        }

        public float Yaw
        {
            get { return this.yaw; }
            set { this.yaw = value; this.markDirty(); }
        }

        public float Roll
        {
            get { return this.roll; }
            set { this.roll = value; this.markDirty(); }
        }

        public Vector3 Translation
        {
            get { return this.translation; }
            set { this.translation = value; this.markDirty(); }
        }

        #endregion

        private void markDirty()
        {
            this.isTransformOutOfDate = true;
        }

        public void Render()
        {
            if (this.isTransformOutOfDate)
            {
                this.recalculateTransform();
            }

            this.transformUniform.Matrix = this.transform;

            this.surface.Render();
        }

        private void recalculateTransform()
        {
            this.transform =
                Matrix4.CreateScale(this.scale) *
                Matrix4.CreateRotationX(this.pitch) *
                Matrix4.CreateRotationY(this.yaw) *
                Matrix4.CreateRotationZ(this.roll) *
                Matrix4.CreateTranslation(this.translation)
                ;
        }
    }
}
