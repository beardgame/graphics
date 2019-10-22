using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// This class represents a GLSL shader program.
    /// </summary>
    public class ShaderProgram : IDisposable, ISurfaceShader
    {
        /// <summary>
        /// The GLSL shader program handle
        /// </summary>
        public int Handle { get; }

        private readonly Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
        private readonly Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public static ShaderProgram FromFiles(string vertexShaderPath, string fragmentShaderPath) =>
            new ShaderProgram(VertexShader.FromFile(vertexShaderPath), FragmentShader.FromFile(fragmentShaderPath));

        public static ShaderProgram FromCode(string vertexShaderCode, string fragmentShaderCode) =>
            new ShaderProgram(new VertexShader(vertexShaderCode), new FragmentShader(fragmentShaderCode));

        /// <summary>
        /// Creates a new shader program.
        /// </summary>
        /// <param name="shaders">The different shaders of the program.</param>
        public ShaderProgram(params Shader[] shaders)
            : this(null, (IEnumerable<Shader>)shaders) {}

        public ShaderProgram(IEnumerable<Shader> shaders)
            : this(null, shaders) {}

        public ShaderProgram(Action<ShaderProgram> preLinkAction, params Shader[] shaders)
            : this(preLinkAction, (IEnumerable<Shader>)shaders) {}

        /// <summary>
        /// Creates a new shader program.
        /// </summary>
        /// <param name="preLinkAction">An action to perform before linking the shader program.</param>
        /// <param name="shaders">The different shaders of the program.</param>
        public ShaderProgram(Action<ShaderProgram> preLinkAction, IEnumerable<Shader> shaders)
        {
            Handle = GL.CreateProgram();

            var shaderList = shaders as IList<Shader> ?? shaders.ToList();

            foreach (var shader in shaderList)
            {
                GL.AttachShader(this, shader);
            }

            preLinkAction?.Invoke(this);

            GL.LinkProgram(this);
            foreach (var shader in shaderList)
            {
                GL.DetachShader(this, shader);
            }

            throwIfLinkingFailed();
        }

        private void throwIfLinkingFailed()
        {
            GL.GetProgram(this, GetProgramParameterName.LinkStatus, out var statusCode);

            if (statusCode == StatusCode.Ok) return;

            GL.GetProgramInfoLog(this, out var info);
            throw new ApplicationException($"Could not link shader: {info}");
        }

        /// <summary>
        /// Sets the vertex attributes.
        /// </summary>
        /// <param name="vertexAttributes">The vertex attributes to set.</param>
        public void SetVertexAttributes(IEnumerable<VertexAttribute> vertexAttributes)
        {
            foreach (var t in vertexAttributes)
                t.SetAttribute(this);
        }

        /// <summary>
        /// Gets an attribute's location.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>The attribute's location, or -1 if not found.</returns>
        public int GetAttributeLocation(string name)
        {
            if (attributeLocations.TryGetValue(name, out var i)) return i;

            i = GL.GetAttribLocation(this, name);
            attributeLocations.Add(name, i);
            return i;
        }

        /// <summary>
        /// Gets a uniform's location.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <returns>The uniform's location, or -1 if not found.</returns>
        public int GetUniformLocation(string name)
        {
            if (uniformLocations.TryGetValue(name, out var i)) return i;

            i = GL.GetUniformLocation(this, name);
            uniformLocations.Add(name, i);
            return i;
        }

        /// <summary>
        /// Sets the surface's shader program to this.
        /// </summary>
        /// <param name="surface">The surface to update the shader program on.</param>
        public void UseOnSurface(Surface surface)
        {
            surface.SetShaderProgram(this);
        }

        /// <summary>
        /// Casts the shader program object to its GLSL program object handle, for easy use with OpenGL functions.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns>GLSL program object handle.</returns>
        public static implicit operator int(ShaderProgram program) => program.Handle;

        public void Dispose()
        {
            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteProgram(this);
        }
    }
}
