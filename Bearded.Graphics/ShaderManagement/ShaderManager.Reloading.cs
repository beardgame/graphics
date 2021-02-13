using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Bearded.Graphics.ShaderManagement
{
    public sealed partial class ShaderManager
    {
        public ShaderReloadReport TryReloadAll()
        {
            var errors = ImmutableList<Exception>.Empty;

            var reloadedShaders = new HashSet<IShaderProvider>(
                shaders.Values
                    .SelectMany(shadersForType => shadersForType.Values)
                    .Where(shader => countReloadsAndListExceptions(shader.ReloadIfNeeded))
                );

            if (reloadedShaders.Count == 0)
            {
                return errors.Count == 0
                    ? ShaderReloadReport.NoChanges
                    : new ShaderReloadReport(0, 0,
                        errors.ToImmutableArray()
                    );
            }

            var reloadedProgramCount = programs.Values.Count(
                p => countReloadsAndListExceptions(() => p.ReloadIfContainsAny(reloadedShaders))
                );

            var report = new ShaderReloadReport(
                reloadedShaders.Count,
                reloadedProgramCount,
                errors.ToImmutableArray()
                );

            reloadedShaders.Clear();

            return report;

            bool countReloadsAndListExceptions(Func<bool> reloadAction)
            {
                try
                {
                    return reloadAction();
                }
                catch (Exception e)
                {
                    errors = errors.Add(e);
                }

                return false;
            }
        }
    }
}
