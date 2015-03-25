using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics.ShaderManagement
{
    sealed class ShaderFileLoader
    {
        private readonly string pathPrefix;
        private readonly string vsExtension;
        private readonly string fsExtension;
        private readonly string gsExtension;
        private readonly bool appendExtensionsForSingleFiles;
        private readonly bool canBlindlyLoadVS;
        private readonly bool canBlindlyLoadFS;
        private readonly bool canBlindlyLoadGS;
        private readonly bool canBlindlyLoadAnything;

        private ShaderFileLoader(string pathPrefix,
            string vsExtension, string fsExtension, string gsExtension,
            bool appendExtensionsForSingleFiles)
        {
            var path = new StringBuilder(pathPrefix, pathPrefix.Length + 1);
            path.Replace('\\', '/');
            if (path[path.Length - 1] != '/')
                path.Append('/');

            this.pathPrefix = path.ToString();
            this.vsExtension = vsExtension;
            this.fsExtension = fsExtension;
            this.gsExtension = gsExtension;
            this.appendExtensionsForSingleFiles = appendExtensionsForSingleFiles;

            this.canBlindlyLoadVS = this.vsExtension != this.fsExtension && this.vsExtension != this.gsExtension;
            this.canBlindlyLoadFS = this.fsExtension != this.vsExtension && this.fsExtension != this.gsExtension;
            this.canBlindlyLoadGS = this.gsExtension != this.fsExtension && this.gsExtension != this.vsExtension;

            this.canBlindlyLoadAnything = this.canBlindlyLoadFS || this.canBlindlyLoadVS || this.canBlindlyLoadGS;
        }

        public ShaderFile Load(string fileName, ShaderType shaderType)
        {
            var path = Path.Combine(this.pathPrefix, fileName);
            if (this.appendExtensionsForSingleFiles)
                path = this.appendExtension(path, shaderType);
            
            return new ShaderFile(shaderType, path, fileName);
        }

        private string appendExtension(string path, ShaderType shaderType)
        {
            switch (shaderType)
            {
                case ShaderType.FragmentShader:
                    return path + this.fsExtension;
                case ShaderType.VertexShader:
                    return path + this.vsExtension;
                case ShaderType.GeometryShader:
                    return path + this.gsExtension;
                default:
                    throw new ArgumentOutOfRangeException("shaderType");
            }
        }

        public IEnumerable<ShaderFile> Load(string path, string searchPattern = "*", bool searchRecursive = true)
        {
            if (!this.canBlindlyLoadAnything)
                return Enumerable.Empty<ShaderFile>();

            var searchPath = Path.Combine(this.pathPrefix, path).Replace(@"\", "/");
            if (searchPath[searchPath.Length - 1] != '/')
                searchPath += "/";

            var shaders = new List<ShaderFile>();

            var searchOption = searchRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (this.canBlindlyLoadVS)
            {
                shaders.AddRange(this.load(ShaderType.VertexShader, searchPath, searchPattern, searchOption));
            }
            if (this.canBlindlyLoadFS)
            {
                shaders.AddRange(this.load(ShaderType.FragmentShader, searchPath, searchPattern, searchOption));
            }
            if (this.canBlindlyLoadGS)
            {
                shaders.AddRange(this.load(ShaderType.GeometryShader, searchPath, searchPattern, searchOption));
            }

            return shaders;
        }

        private IEnumerable<ShaderFile> load(ShaderType type, string searchPath,
            string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(searchPath, searchPattern, searchOption)
                .Select(
                    f => new ShaderFile(type, f, this.getFriendlyName(searchPath, f))
                );
        }

        private string getFriendlyName(string prefix, string fullPath)
        {
            return Path.ChangeExtension(fullPath.Replace(@"\", "/").Substring(prefix.Length), null);
        }

        #region Building

        public static ShaderFileLoader CreateDefault(string pathPrefix = "")
        {
            return new ShaderFileLoader(pathPrefix ?? "", ".vs", ".fs", ".gs", true);
        }

        public class Builder
        {
            public string PathPrefix { get; set; }
            public string VertexShaderFileExtension { get; set; }
            public string FragmentShaderFileExtension { get; set; }
            public string GeometryShaderFileExtension { get; set; }

            public bool AppendExtensionForSingleFiles { get; set; }

            public Builder(string pathPrefix = "")
            {
                this.PathPrefix = pathPrefix;
                this.VertexShaderFileExtension = ".vs";
                this.FragmentShaderFileExtension = ".fs";
                this.GeometryShaderFileExtension = ".gs";
                this.AppendExtensionForSingleFiles = true;
            }

            public Builder DontAppendExtensionsForSingleFile()
            {
                this.AppendExtensionForSingleFiles = false;
                return this;
            }

            public Builder WithExtensions(string vertexShaderFileExtension = null,
                string fragmentShaderFileExtension = null, string geometryShaderFileExtension = null)
            {
                this.VertexShaderFileExtension = vertexShaderFileExtension ?? this.VertexShaderFileExtension;
                this.FragmentShaderFileExtension = fragmentShaderFileExtension ?? this.FragmentShaderFileExtension;
                this.GeometryShaderFileExtension = geometryShaderFileExtension ?? this.GeometryShaderFileExtension;
                return this;
            }

            public ShaderFileLoader Build()
            {
                return new ShaderFileLoader(
                    this.PathPrefix ?? "",
                    this.VertexShaderFileExtension ?? "",
                    this.FragmentShaderFileExtension ?? "",
                    this.GeometryShaderFileExtension ?? "",
                    this.AppendExtensionForSingleFiles
                    );
            }
        }

        #endregion
    }
}
