using OpenToolkit.Graphics.OpenGL;

namespace amulware.Graphics
{
    public class PolygonModeSetting : SurfaceSetting
    {
        private readonly PolygonMode mode;
        private readonly MaterialFace face;

        public PolygonModeSetting(PolygonMode mode)
            : this(mode, MaterialFace.FrontAndBack)
        {
        }
        public PolygonModeSetting(PolygonMode mode, MaterialFace face)
            : base(true)
        {
            this.mode = mode;
            this.face = face;
        }

        public override void Set(ShaderProgram program)
        {
            GL.PolygonMode(this.face, this.mode);
        }

        public override void UnSet(ShaderProgram program)
        {
            GL.PolygonMode(this.face, PolygonMode.Fill);
        }
    }
}

