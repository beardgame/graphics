using Newtonsoft.Json;
using System;
using System.IO;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Serialization.JsonNet
{
    class UVRectangleConverter : JsonConverterBase<UVRectangle>
    {
        public Vector2 Scalar { get; set; }

        public UVRectangleConverter()
        {
            this.Scalar = new Vector2(1, 1);
        }

        /// <summary>
        /// Reads a string from the reader, and converts it to a uv rectangle.
        /// </summary>
        /// <param name="reader">The JSON reader to fetch data from.</param>
        /// <param name="serializer">The serializer for embedded serialization.</param>
        /// <returns>The <see cref="UVRectangleContainer"/> identified in the JSON.</returns>
        protected override UVRectangle readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            bool absolute = true;
            UVRectangle uv = new UVRectangle();

            bool sizeGiven = false;
            Vector2 size = new Vector2();

            float rotation = 0;

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
                    case "absolute":
                        absolute = serializer.Deserialize<bool>(reader);
                        break;
                    case "leftTop":
                    case "lt":
                        uv.TopLeft = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "leftBottom":
                    case "lb":
                        uv.BottomLeft = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "rightTop":
                    case "rt":
                        uv.TopRight = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "rightBottom":
                    case "rb":
                        uv.BottomRight = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "size":
                        size = serializer.Deserialize<Vector2>(reader);
                        sizeGiven = true;
                        break;
                    case "rotation":
                        rotation = serializer.Deserialize<float>(reader);
                        break;
                    default:
                        throw new InvalidDataException(String.Format("Unknown property while deserialising uv rectangle: {0}", propertyName));
                }
            }

            if (sizeGiven)
                uv = new UVRectangle(uv.TopLeft.X, uv.TopLeft.X + size.X, uv.TopLeft.Y, uv.TopLeft.Y + size.Y);

            if (rotation != 0)
                uv.Rotate(rotation);

            uv.ReScale(this.Scalar);

            return uv;
        }

        /// <summary>
        /// Converts the given <see cref="UVRectangleContainer"/> to JSON.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="value">The value to convert</param>
        /// <param name="serializer">Unused by this serializer</param>
        protected override void writeJsonImpl(JsonWriter writer, UVRectangle value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Json serializing of type UVRectangleContainer not yet implemented.");
        }
    }
}
