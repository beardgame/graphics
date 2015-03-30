using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    sealed public class ShaderManager
    {
        private readonly Dictionary<ReloadableShader, string> shaderNames
            = new Dictionary<ReloadableShader, string>();

        private readonly Dictionary<ShaderType, Dictionary<string, ReloadableShader>> shaders
            = new Dictionary<ShaderType, Dictionary<string, ReloadableShader>>(3)
            {
                { ShaderType.VertexShader, new Dictionary<string, ReloadableShader>() },
                { ShaderType.GeometryShader, new Dictionary<string, ReloadableShader>() },
                { ShaderType.FragmentShader, new Dictionary<string, ReloadableShader>() },
            };

        private readonly Dictionary<string, ReloadableShaderProgram> programs
            = new Dictionary<string, ReloadableShaderProgram>();

        private readonly Dictionary<ReloadableShader, List<ReloadableShaderProgram>> programsByShader
            = new Dictionary<ReloadableShader, List<ReloadableShaderProgram>>();

        private readonly HashSet<ReloadableShader> reloadedShaders = new HashSet<ReloadableShader>();

        public enum ReloadReport
        {
            None = 0,
            Overview,
            Detailed
        }

        public ReloadReport ReloadReportType { get; set; }

        #region TryReload()

        public void TryReloadAll()
        {
            foreach (var shader in this.shaders.Values.SelectMany(d => d.Values))
            {
                if (this.tryReloadShader(shader))
                    this.reloadedShaders.Add(shader);
            }

            if (this.reloadedShaders.Count == 0)
                return;

            var reloadedPrograms = programs.Values.Count(this.tryReloadProgramIfContainsReloadedShaders);

            this.reloadedShaders.Clear();
        }

        public void TryReload(string shaderProgramName, bool reloadProgramsSharingShaders = true)
        {
            ReloadableShaderProgram program;
            if(!this.programs.TryGetValue(shaderProgramName, out program))
                throw new ArgumentException(string.Format("Shader program with name '{0}' not found.", shaderProgramName));

            HashSet<ReloadableShaderProgram> programsToReload = null;

            if(reloadProgramsSharingShaders)
                programsToReload = new HashSet<ReloadableShaderProgram>();

            var reloadedAny = false;

            foreach (var shader in program.Shaders)
            {
                if (!this.tryReloadShader(shader))
                    continue;

                reloadedAny = true;
                if (reloadProgramsSharingShaders)
                {
                    foreach (var p in this.programsByShader[shader])
                    {
                        programsToReload.Add(p);
                    }
                }
                else
                {
                    // make sure shader is marked reloaded for next TryReloadAll call
                    this.reloadedShaders.Add(shader);
                }
            }

            if (reloadedAny)
            {
                var reloadedPrograms = 0;
                if (reloadProgramsSharingShaders)
                {
                    reloadedPrograms = programsToReload.Count(this.tryReloadProgram);
                }
                else
                {
                    if (this.tryReloadProgram(program))
                        reloadedPrograms = 1;
                }
            }
        }

        public void TryReload(ShaderType type, string shaderName)
        {
            var shader = this[type, shaderName];

            if(shader == null)
                throw new ArgumentException(string.Format("{0} with name '{1}' not found.", type, shaderName));

            if(!this.tryReloadShader(shader))
                return;

            // remove in case it was added earlier to avoid double reloading
            this.reloadedShaders.Remove(shader);

            // shader was reloaded successfully, attempt to reload programs

            List<ReloadableShaderProgram> programs;
            this.programsByShader.TryGetValue(shader, out programs);

            if (programs == null)
                // shader has no programs
                return;

            var reloadedPrograms = programs.Count(this.tryReloadProgram);
        }


        private bool tryReloadProgram(ReloadableShaderProgram program)
        {       
            try
            {
                program.Reload();
                
                return true;
            }
            catch (Exception e)
            {
                // linking program with new shader failed
            }
            return false;
        }

        private bool tryReloadProgramIfContainsReloadedShaders(ReloadableShaderProgram program)
        {
            try
            {
                program.ReloadIfContainsAny(this.reloadedShaders);

                return true;
            }
            catch (Exception e)
            {
                // linking program with new shader failed
            }
            return false;
        }

        private bool tryReloadShader(ReloadableShader shader)
        {
            try
            {
                return shader.TryReload();
            }
            catch (Exception e)
            {
                // reloading shader failed, probably invalid file or code
            }
            return false;
        }

        #endregion

        #region Add()

        public void Add(IEnumerable<ShaderFile> shaderFiles)
        {
            foreach (var file in shaderFiles)
            {
                this.Add(file);
            }
        }

        public void Add(ShaderFile shaderFile)
        {
            this.Add(shaderFile, shaderFile.FriendlyName);
        }

        public void Add(IShaderReloader shader, string name)
        {
            this.Add(new ReloadableShader(shader), name);
        }

        public void Add(ReloadableShader shader, string name)
        {
            if(this.shaderNames.ContainsKey(shader))
                throw new ArgumentException(string.Format("Shader already known by name '{0}'.", this.shaderNames[shader]));

            var dict = this.shaders[shader.Type];
            dict.Add(name, shader); // will throw if name already taken

            this.shaderNames.Add(shader, name);
        }

        public void Add(ReloadableShaderProgram shaderProgram, string name)
        {
            var knownShaders = new bool[shaderProgram.Shaders.Count];

            // make sure all shaders can be added so in case of error our internal state stays valid
            for (int i = 0; i < shaderProgram.Shaders.Count; i++)
            {
                var shader = shaderProgram.Shaders[i];
                if (this.shaderNames.ContainsKey(shader))
                {
                    knownShaders[i] = true;
                }
                else
                {
                    var dict = this.shaders[shader.Type];
                    if (dict.ContainsKey(name))
                    {
                        throw new ArgumentException(
                            string.Format("Tried adding unknown {0} under name '{1}', but name already taken.", 
                                shader.Type, name));
                    }
                }
            }

            // add shaders we don't know yet
            for (int i = 0; i < shaderProgram.Shaders.Count; i++)
            {
                if (!knownShaders[i])
                {
                    var shader = shaderProgram.Shaders[i];
                    this.Add(shader, name);
                }
            }

            this.programs.Add(name, shaderProgram);
            
            // register program for all shaders
            foreach (var shader in shaderProgram.Shaders)
            {
                List<ReloadableShaderProgram> programs;
                this.programsByShader.TryGetValue(shader, out programs);
                if (programs == null)
                {
                    programs = new List<ReloadableShaderProgram>();
                    this.programsByShader.Add(shader, programs);
                }
                programs.Add(shaderProgram);
            }
        }

        #endregion

        #region this[]

        public ISurfaceShader this[string shaderProgramName]
        {
            get
            {
                ReloadableShaderProgram program;
                this.programs.TryGetValue(shaderProgramName, out program);
                return program;
            }
        }

        public ReloadableShader this[ShaderType type, string shaderName]
        {
            get
            {
                ReloadableShader shader;
                this.shaders[type].TryGetValue(shaderName, out shader);
                return shader;
            }
        }

        #endregion

        #region ProgramBuilding

        public ProgramBuilder MakeShaderProgram()
        {
            return new ProgramBuilder(this);
        }

        public sealed class ProgramBuilder
        {
            private readonly ShaderManager manager;
            private readonly List<ReloadableShader> shaders = new List<ReloadableShader>();

            public ProgramBuilder(ShaderManager manager)
            {
                this.manager = manager;
            }

            public ProgramBuilder With(ReloadableShader shader)
            {
                this.shaders.Add(shader);
                return this;
            }

            public ProgramBuilder With(ShaderType type, string shaderName)
            {
                var shader = this.manager[type, shaderName];
                if(shader == null)
                    throw new ArgumentException(string.Format("{0} with name '{1}' not found.", type, shaderName));
                return this.With(shader);
            }

            public ProgramBuilder WithVertexShader(string shaderName)
            {
                return this.With(ShaderType.VertexShader, shaderName);
            }
            public ProgramBuilder WithFragmentShader(string shaderName)
            {
                return this.With(ShaderType.FragmentShader, shaderName);
            }
            public ProgramBuilder WithGeometryShader(string shaderName)
            {
                return this.With(ShaderType.GeometryShader, shaderName);
            }

            public ISurfaceShader As(string programName)
            {
                var program = new ReloadableShaderProgram(this.shaders);
                this.manager.Add(program, programName);
                return program;
            }
        }

        #endregion

    }
}
