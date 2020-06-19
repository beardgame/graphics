using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Serialization.JsonNet
{
    internal class SpriteSetConverter<TVertexData> : JsonConverterBase<SpriteSet<TVertexData>>
        where TVertexData : struct, IVertexData
    {
        private readonly Func<IndexedSurface<TVertexData>, UVQuadGeometry<TVertexData>> geometryMaker;
        private readonly ISurfaceShader shaderProgram;
        private readonly SurfaceSetting[] surfaceSettings;
        private readonly Func<string, Texture> textureProvider;

        public SpriteSetConverter(ISurfaceShader shaderProgram, SurfaceSetting[] surfaceSettings,
            Func<IndexedSurface<TVertexData>, UVQuadGeometry<TVertexData>> geometryMaker, Func<string, Texture> textureProvider = null)
        {
            this.shaderProgram = shaderProgram;
            this.surfaceSettings = surfaceSettings;
            this.geometryMaker = geometryMaker;
            this.textureProvider = textureProvider ?? (s => new Texture(s));
        }

        protected override SpriteSet<TVertexData> readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            var set = new SpriteSet<TVertexData>(this.shaderProgram, this.surfaceSettings);

            var rectangleConverter = new UVRectangleConverter();
            serializer.Converters.Add(rectangleConverter);

            var spriteConverter = new SpriteConverter<TVertexData>(() => this.geometryMaker(set.Surface));
            serializer.Converters.Add(spriteConverter);

            int textureCount = 0;

            while (reader.Read())
            {
                // break on unexpected or end of object
                if (reader.TokenType != JsonToken.PropertyName)
                    break;

                var propertyName = (string)reader.Value;

                if (!reader.Read())
                    // no property value? stop reading and let JSON.NET fail
                    break;

                // read correct property
                switch (propertyName)
                {
                    case "textures":
                        var texConverter = new TupleConverter<string, string>("name", "filename");
                        serializer.Converters.Add(texConverter);
                        var textures = serializer.Deserialize<List<Tuple<string, string>>>(reader);
                        serializer.Converters.Remove(texConverter);
                        textures.ForEach(
                            t => set.Surface.AddSetting(new TextureUniform(t.Item1, this.textureProvider(t.Item2),
                                OpenToolkit.Graphics.OpenGL.TextureUnit.Texture0 + textureCount++))
                                );
                        break;
                    case "uvSize":
                        Vector2 invScalar = serializer.Deserialize<Vector2>(reader);
                        rectangleConverter.Scalar = new Vector2(1f / invScalar.X, 1f / invScalar.Y);
                        break;
                    case "sprites":
                        var sprites = serializer.Deserialize<List<Sprite<TVertexData>>>(reader);
                        sprites.ForEach(s => set.Sprites.Add(s.Name, s));
                        break;
                    default:
                        throw new InvalidDataException(String.Format("Unknown property while deserialising sprite set: {0}", propertyName));
                }

            }

            return set;
        }

        protected override void writeJsonImpl(JsonWriter writer, SpriteSet<TVertexData> value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Json serializing of generic type SpriteSet not yet implemented.");
        }
    }
}
