using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Graphics.ShaderManagement
{
    // TODO: fix this up
    public sealed class ShaderFileLoader
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
            if (path[^1] != '/')
                path.Append('/');

            this.pathPrefix = path.ToString();
            this.vsExtension = vsExtension;
            this.fsExtension = fsExtension;
            this.gsExtension = gsExtension;
            this.appendExtensionsForSingleFiles = appendExtensionsForSingleFiles;

            canBlindlyLoadVS = this.vsExtension != this.fsExtension && this.vsExtension != this.gsExtension;
            canBlindlyLoadFS = this.fsExtension != this.vsExtension && this.fsExtension != this.gsExtension;
            canBlindlyLoadGS = this.gsExtension != this.fsExtension && this.gsExtension != this.vsExtension;

            canBlindlyLoadAnything = canBlindlyLoadFS || canBlindlyLoadVS || canBlindlyLoadGS;
        }

        public ShaderFile Load(string fileName, ShaderType shaderType)
        {
            var path = Path.Combine(pathPrefix, fileName);
            if (appendExtensionsForSingleFiles)
                path = appendExtension(path, shaderType);

            return new ShaderFile(shaderType, path, fileName);
        }

        public IEnumerable<ShaderFile> Load(string path, string searchPattern = "*", bool searchRecursive = true)
        {
            if (!canBlindlyLoadAnything)
                return Enumerable.Empty<ShaderFile>();

            var searchPath = Path.Combine(pathPrefix, path).Replace(@"\", "/");
            if (searchPath[^1] != '/')
                searchPath += "/";

            var shaders = new List<ShaderFile>();

            var searchOption = searchRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (canBlindlyLoadVS)
            {
                shaders.AddRange(load(ShaderType.VertexShader,
                    searchPath, searchPattern + vsExtension, searchOption));
            }
            if (canBlindlyLoadFS)
            {
                shaders.AddRange(load(ShaderType.FragmentShader,
                    searchPath, searchPattern + fsExtension, searchOption));
            }
            if (canBlindlyLoadGS)
            {
                shaders.AddRange(load(ShaderType.GeometryShader,
                    searchPath, searchPattern + gsExtension, searchOption));
            }

            return shaders;
        }

        private IEnumerable<ShaderFile> load(ShaderType type, string searchPath,
            string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(searchPath, searchPattern, searchOption)
                .Select(
                    f => new ShaderFile(type, f, getFriendlyName(searchPath, f))
                );
        }

        private string getFriendlyName(string prefix, string fullPath)
        {
            return Path.ChangeExtension(fullPath.Replace(@"\", "/")[prefix.Length..], null);
        }

        private string appendExtension(string path, ShaderType shaderType)
        {
            switch (shaderType)
            {
                case ShaderType.FragmentShader:
                    return path + fsExtension;
                case ShaderType.VertexShader:
                    return path + vsExtension;
                case ShaderType.GeometryShader:
                    return path + gsExtension;
                default:
                    throw new ArgumentOutOfRangeException("shaderType");
            }
        }

        public static ShaderFileLoader CreateDefault(string pathPrefix = "")
        {
            return new ShaderFileLoader(pathPrefix, ".vs", ".fs", ".gs", true);
        }

        public class Builder
        {
            public string? PathPrefix { get; set; }
            public string? VertexShaderFileExtension { get; set; }
            public string? FragmentShaderFileExtension { get; set; }
            public string? GeometryShaderFileExtension { get; set; }

            public bool AppendExtensionForSingleFiles { get; set; }

            public Builder(string pathPrefix = "")
            {
                PathPrefix = pathPrefix;
                VertexShaderFileExtension = ".vs";
                FragmentShaderFileExtension = ".fs";
                GeometryShaderFileExtension = ".gs";
                AppendExtensionForSingleFiles = true;
            }

            public Builder DontAppendExtensionsForSingleFile()
            {
                AppendExtensionForSingleFiles = false;
                return this;
            }

            public Builder WithExtensions(string? vertexShaderFileExtension = null,
                string? fragmentShaderFileExtension = null, string? geometryShaderFileExtension = null)
            {
                VertexShaderFileExtension = vertexShaderFileExtension ?? VertexShaderFileExtension;
                FragmentShaderFileExtension = fragmentShaderFileExtension ?? FragmentShaderFileExtension;
                GeometryShaderFileExtension = geometryShaderFileExtension ?? GeometryShaderFileExtension;
                return this;
            }

            public ShaderFileLoader Build()
            {
                return new ShaderFileLoader(
                    PathPrefix ?? "",
                    VertexShaderFileExtension ?? "",
                    FragmentShaderFileExtension ?? "",
                    GeometryShaderFileExtension ?? "",
                    AppendExtensionForSingleFiles
                    );
            }
        }
    }
}
