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

        private readonly CachedVariableLocator attributeLocations;
        private readonly CachedVariableLocator uniformLocations;

        public static ShaderProgram FromFiles(string vertexShaderPath, string fragmentShaderPath) =>
            FromShaders(VertexShader.FromFile(vertexShaderPath), FragmentShader.FromFile(fragmentShaderPath));

        public static ShaderProgram FromCode(string vertexShaderCode, string fragmentShaderCode) =>
            FromShaders(VertexShader.FromCode(vertexShaderCode), VertexShader.FromCode(fragmentShaderCode));
        
        public static ShaderProgram FromShaders(params Shader[] shaders) => new ShaderProgram(shaders);
        
        public static ShaderProgram FromShaders(IEnumerable<Shader> shaders) => new ShaderProgram(shaders);

        /// <summary>
        /// Creates a new shader program.
        /// </summary>
        /// <param name="shaders">The different shaders of the program.</param>
        private ShaderProgram(IEnumerable<Shader> shaders)
        {
            Handle = GL.CreateProgram();

            var shaderList = shaders as IList<Shader> ?? shaders.ToList();

            foreach (var shader in shaderList)
            {
                GL.AttachShader(this, shader);
            }

            GL.LinkProgram(this);
            foreach (var shader in shaderList)
            {
                GL.DetachShader(this, shader);
            }

            throwIfLinkingFailed();
            
            attributeLocations = new CachedVariableLocator(name => GL.GetAttribLocation(Handle, name));
            uniformLocations = new CachedVariableLocator(name => GL.GetUniformLocation(Handle, name));
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
        public int GetAttributeLocation(string name) => attributeLocations.GetVariableLocation(name);

        /// <summary>
        /// Gets a uniform's location.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <returns>The uniform's location, or -1 if not found.</returns>
        public int GetUniformLocation(string name) => uniformLocations.GetVariableLocation(name);

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
