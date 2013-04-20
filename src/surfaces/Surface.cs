using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    /// <summary>
    /// Base class for all surfaces. A Surface is an abstract object that can render itself to the screen using a shader program and can have a number of settings to modify its behaviour.
    /// </summary>
    abstract public class Surface
    {
        protected ShaderProgram program;

        private readonly List<SurfaceSetting> settingsSet = new List<SurfaceSetting>();
        private readonly List<SurfaceSetting> settingsUnSet = new List<SurfaceSetting>();

        /// <summary>
        /// Sets the shader program used to render this surface.
        /// </summary>
        /// <param name="program">The program.</param>
        public void SetShaderProgram(ShaderProgram program)
        {
            this.program = program;
            this.onNewShaderProgram();
        }

        /// <summary>
        /// Is called after a shader program has been set. Use this for sub class specific behaviour.
        /// </summary>
        protected virtual void onNewShaderProgram() { }

        /// <summary>
        /// Adds <see cref="SurfaceSetting"/>s to this surface.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void AddSettings(IEnumerable<SurfaceSetting> settings)
        {
            foreach (SurfaceSetting setting in settings)
                this.AddSetting(setting);
        }

        /// <summary>
        /// Adds <see cref="SurfaceSetting"/>s to this surface.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void AddSettings(params SurfaceSetting[] settings)
        {
            foreach (SurfaceSetting setting in settings)
                this.AddSetting(setting);
        }

        /// <summary>
        /// Adds a <see cref="SurfaceSetting"/> to this surface.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public void AddSetting(SurfaceSetting setting)
        {
            this.settingsSet.Add(setting);
            if (setting.NeedsUnsetting)
                this.settingsUnSet.Add(setting);
        }

        /// <summary>
        /// Removes a <see cref="SurfaceSetting"/> from this surface.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public void RemoveSetting(SurfaceSetting setting)
        {
            if (this.settingsSet.Remove(setting) && setting.NeedsUnsetting)
                this.settingsUnSet.Remove(setting);
        }

        /// <summary>
        /// Removes all <see cref="SurfaceSetting"/>s from this surface.
        /// </summary>
        public void ClearSettings()
        {
            this.settingsSet.Clear();
            this.settingsUnSet.Clear();
        }

        /// <summary>
        /// Renders this surface.
        /// It does so by activating its shader program, setting all its settings, invoking sub class specific render behaviour and then unsetting its settings again.
        /// </summary>
        public void Render()
        {
            GL.UseProgram(this.program);

            foreach (SurfaceSetting setting in this.settingsSet)
                setting.Set(this.program);
            
            this.render();

            foreach (SurfaceSetting setting in this.settingsUnSet)
                setting.UnSet(this.program);
        }

        /// <summary>
        /// Is called after the surface's shader program and its settings have been set. Implement this to add sub class specific render behaviour.
        /// </summary>
        abstract protected void render();
    }
}
