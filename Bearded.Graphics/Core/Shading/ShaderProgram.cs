using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.Shading
{
    public sealed class ShaderProgram : IDisposable
    {
        private readonly int handle;

        private readonly CachedVariableLocator attributeLocations;
        private readonly CachedVariableLocator uniformLocations;

        public static ShaderProgram FromShaders(params Shader[] shaders) => new(shaders);

        public static ShaderProgram FromShaders(IEnumerable<Shader> shaders) => new(shaders);

        private ShaderProgram(IEnumerable<Shader> shaders)
        {
            handle = GL.CreateProgram();

            var shaderList = shaders as IList<Shader> ?? shaders.ToList();

            foreach (var shader in shaderList)
            {
                GL.AttachShader(handle, shader.Handle);
            }

            GL.LinkProgram(handle);

            foreach (var shader in shaderList)
            {
                GL.DetachShader(handle, shader.Handle);
            }

            throwIfLinkingFailed();

            attributeLocations = new CachedVariableLocator(name => GL.GetAttribLocation(handle, name));
            uniformLocations = new CachedVariableLocator(name => GL.GetUniformLocation(handle, name));
        }

        private void throwIfLinkingFailed()
        {
            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out var statusCode);

            if (statusCode == StatusCode.Ok) return;

            GL.GetProgramInfoLog(handle, out var info);
            throw new ApplicationException($"Could not link shader: {info}");
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

        public Using Use()
        {
            return new Using(in handle);
        }

        public struct Using : IDisposable
        {
            public Using(in int handle)
            {
                GL.UseProgram(handle);
            }

            public void Dispose()
            {
                GL.UseProgram(0);
            }
        }

        public void Dispose()
        {
            GL.DeleteProgram(handle);
        }
    }
}
