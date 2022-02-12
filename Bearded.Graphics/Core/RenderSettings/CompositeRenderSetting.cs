using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics.Shading;

namespace Bearded.Graphics.RenderSettings
{
    public sealed class CompositeRenderSetting : IRenderSetting
    {
        private readonly ImmutableArray<IRenderSetting> settings;

        public CompositeRenderSetting(IEnumerable<IRenderSetting> settings)
        {
            this.settings = settings.ToImmutableArray();
        }

        public void SetForProgram(ShaderProgram program)
        {
            foreach (var setting in settings)
            {
                setting.SetForProgram(program);
            }
        }

        public IProgramRenderSetting ForProgram(ShaderProgram program)
        {
            return new ProgramSetting(settings.Select(s => s.ForProgram(program)));
        }

        private sealed class ProgramSetting : IProgramRenderSetting
        {
            private readonly ImmutableArray<IProgramRenderSetting> settings;

            public ProgramSetting(IEnumerable<IProgramRenderSetting> settings)
            {
                this.settings = settings.ToImmutableArray();
            }

            public void Set()
            {
                foreach (var setting in settings)
                {
                    setting.Set();
                }
            }
        }
    }
}
