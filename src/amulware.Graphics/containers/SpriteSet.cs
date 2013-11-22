using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using amulware.Graphics.Serialization.JsonNet;

namespace amulware.Graphics
{
    
    public class SpriteSet<TVertexData> : ISpriteSet<TVertexData>
        where TVertexData : struct, IVertexData
    {
        private readonly QuadSurface<TVertexData> surface;

        public QuadSurface<TVertexData> Surface { get { return this.surface; } }

        private readonly Dictionary<string, Sprite<TVertexData>> sprites;

        public Dictionary<string, Sprite<TVertexData>> Sprites { get { return this.sprites; } }

        public Sprite<TVertexData> this[string spriteName]
        {
            get {
                Sprite<TVertexData> s;
                this.sprites.TryGetValue(spriteName, out s);
                return s;
            }
        }

        public SpriteSet(ShaderProgram shaderProgram, SurfaceSetting[] surfaceSettings)
        {
            this.sprites = new Dictionary<string, Sprite<TVertexData>>();
            this.surface = new QuadSurface<TVertexData>();
            if (surfaceSettings != null)
                this.surface.AddSettings(surfaceSettings);
            if (shaderProgram != null)
                this.surface.SetShaderProgram(shaderProgram);
        }

        public static SpriteSet<TVertexData> Copy<TVertexDataIn>
            (SpriteSet<TVertexDataIn> template, Func<QuadSurface<TVertexData>, UVQuadGeometry<TVertexData>> geometryMaker,
            ShaderProgram shaderProgram = null, SurfaceSetting[] surfaceSettings = null, bool keepTextureUniforms = true)
            where TVertexDataIn : struct, IVertexData
        {
            var set = new SpriteSet<TVertexData>(shaderProgram, surfaceSettings);

            foreach(var item in template.sprites)
            {
                set.sprites.Add(item.Key, Sprite<TVertexData>
                .Copy(item.Value, geometryMaker(set.surface)));
            }

            if (keepTextureUniforms)
                set.surface.AddSettings(template.surface.Settings.Where(setting => setting is TextureUniform));

            return set;
        }

        static public SpriteSet<TVertexData> FromJsonFile(
            string filename, Func<QuadSurface<TVertexData>, UVQuadGeometry<TVertexData>> geometryMaker,
            ShaderProgram shaderProgram = null, SurfaceSetting[] surfaceSettings = null,
            Func<string, Texture> textureProvider = null, bool texturesRelativeToJson = false)
        {
            if (textureProvider == null)
                textureProvider = file => new Texture(file);

            if (texturesRelativeToJson)
            {
                string path = Path.GetDirectoryName(filename) ?? "";
                var providerCopy = textureProvider;
                textureProvider = file => providerCopy(Path.Combine(path, file));
            }

            var jsonSettings = new JsonSerializerSettings().ConfigureForGraphics();
            jsonSettings.Converters.Add(
                new SpriteSetConverter<TVertexData>(shaderProgram, surfaceSettings, geometryMaker, textureProvider)
                    );

            var set = JsonConvert.DeserializeObject<SpriteSet<TVertexData>>(
                File.ReadAllText(filename), jsonSettings);

            return set;
        }
    }
}
