using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using amulware.Graphics.Serialization.JsonNet;

namespace amulware.Graphics
{
    
    public class SpriteSet<TVertexData>
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

        static public SpriteSet<TVertexData> FromJsonFile<TVertexData>(
            string filename, Func<QuadSurface<TVertexData>, UVQuadGeometry<TVertexData>> geometryMaker,
            ShaderProgram shaderProgram = null, SurfaceSetting[] surfaceSettings = null)
            where TVertexData : struct, IVertexData
        {
            var jsonSettings = new JsonSerializerSettings().ConfigureForGraphics();
            jsonSettings.Converters.Add(
                new SpriteSetConverter<TVertexData>(shaderProgram, surfaceSettings, geometryMaker)
                    );

            var set = JsonConvert.DeserializeObject<SpriteSet<TVertexData>>(
                File.ReadAllText(filename), jsonSettings);

            return set;
        }
    }
}
