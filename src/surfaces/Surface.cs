using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    abstract public class Surface
    {
        protected ShaderProgram program;

        private readonly List<SurfaceSetting> settingsSet = new List<SurfaceSetting>();
        private readonly List<SurfaceSetting> settingsUnSet = new List<SurfaceSetting>();

        public void SetShaderProgram(ShaderProgram program)
        {
            this.program = program;
            this.onNewShaderProgram();
        }

        protected virtual void onNewShaderProgram() { }

        public void AddSettings(IEnumerable<SurfaceSetting> settings)
        {
            foreach (SurfaceSetting setting in settings)
                this.AddSetting(setting);
        }

        public void AddSettings(params SurfaceSetting[] settings)
        {
            foreach (SurfaceSetting setting in settings)
                this.AddSetting(setting);
        }

        public void AddSetting(SurfaceSetting setting)
        {
            this.settingsSet.Add(setting);
            if (setting.NeedsUnsetting)
                this.settingsUnSet.Add(setting);
        }

        public void RemoveSetting(SurfaceSetting setting)
        {
            if (this.settingsSet.Remove(setting) && setting.NeedsUnsetting)
                this.settingsUnSet.Remove(setting);
        }

        public void ClearSettings()
        {
            this.settingsSet.Clear();
            this.settingsUnSet.Clear();
        }



        public void Render()
        {
            GL.UseProgram(this.program);

            foreach (SurfaceSetting setting in this.settingsSet)
                setting.Set(this.program);
            
            this.render();

            foreach (SurfaceSetting setting in this.settingsUnSet)
                setting.UnSet(this.program);
        }

        abstract protected void render();
    }
}
