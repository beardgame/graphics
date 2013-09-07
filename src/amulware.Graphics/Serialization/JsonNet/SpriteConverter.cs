using Newtonsoft.Json;
using System;
using System.IO;

namespace amulware.Graphics.Serialization.JsonNet
{
    internal class SpriteConverter<TVertexData> : JsonConverterBase<Sprite<TVertexData>>
        where TVertexData : struct, IVertexData
    {
        private Func<UVQuadGeometry<TVertexData>> geometryMaker;

        public SpriteConverter(Func<UVQuadGeometry<TVertexData>> geometryMaker)
        {
            this.geometryMaker = geometryMaker;
        }

        protected override Sprite<TVertexData> readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            float duration = 1;
            string name = null;
            UVRectangle[] uvs = null;

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
                    case "name":
                        name = serializer.Deserialize<string>(reader);
                        break;
                    case "duration":
                        duration = serializer.Deserialize<float>(reader);
                        break;
                    case "uv":
                        uvs = serializer.Deserialize<UVRectangle[]>(reader);
                        break;
                    default:
                        throw new InvalidDataException(String.Format("Unknown property while deserialising sprite: {0}", propertyName));
                }
            }

            if (name == null || name == "")
                throw new InvalidDataException("Sprite must have a name!");

            return new Sprite<TVertexData>(name, uvs, duration, geometryMaker());
        }

        protected override void writeJsonImpl(JsonWriter writer, Sprite<TVertexData> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
