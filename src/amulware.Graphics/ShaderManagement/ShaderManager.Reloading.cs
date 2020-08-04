using System;
using System.Collections.Generic;
using System.Linq;

namespace amulware.Graphics.ShaderManagement
{
    public sealed partial class ShaderManager
    {
        public ShaderReloadReport TryReloadAll()
        {
            var errors = new List<Exception>();
            
            var reloadedShaders = new HashSet<ReloadableShader>(
                shaders.Values
                    .SelectMany(shadersForType => shadersForType.Values)
                    .Where(shader => succeedsWithoutError(shader.ReloadIfNeeded))
                );

            if (reloadedShaders.Count == 0)
            {
                return errors.Count == 0
                    ? ShaderReloadReport.NoChanges
                    : new ShaderReloadReport(0, 0,
                        errors.AsReadOnly()
                    );
            }

            var reloadedProgramCount = programs.Values.Count(
                p => succeedsWithoutError(() => p.ReloadIfContainsAny(reloadedShaders))
                );

            var report = new ShaderReloadReport(
                reloadedShaders.Count,
                reloadedProgramCount,
                errors.AsReadOnly()
                );
            
            reloadedShaders.Clear();

            return report;

            bool succeedsWithoutError(Func<bool> action)
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    errors.Add(e);
                }

                return false;
            }
        }
    }
}
