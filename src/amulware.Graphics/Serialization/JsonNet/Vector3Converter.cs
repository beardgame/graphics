using System;
using System.IO;
using Newtonsoft.Json;
using OpenTK;

namespace amulware.Graphics.Serialization.JsonNet
{
    /// <summary>
    /// Json.NET converter for <see cref="Vector2"/>.
    /// </summary>
    internal sealed class Vector3Converter : JsonConverterBase<Vector3>
    {
        /// <summary>
        /// Reads a string from the reader, and converts it to a Vector2.
        /// </summary>
        /// <param name="reader">The JSON reader to fetch data from.</param>
        /// <param name="serializer">The serializer for embedded serialization.</param>
        /// <returns>The <see cref="Vector2"/> identified in the JSON, or null.</returns>
        protected override Vector3 readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            if (reader.Read() && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
            {
                float x = Convert.ToSingle(reader.Value);
                if (reader.Read() && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                {
                    float y = Convert.ToSingle(reader.Value);
                    if (reader.Read() && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                    {
                        float z = Convert.ToSingle(reader.Value);
                        if (reader.Read() && reader.TokenType == JsonToken.EndArray)
                            return new Vector3(x, y, z);
                    }
                }
            }
            throw new InvalidDataException("A Vector3 must be an array of three numbers.");
        }

        /// <summary>
        /// Converts the given <see cref="Vector2"/> to JSON.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="value">The value to convert</param>
        /// <param name="serializer">Unused by this serializer</param>
        protected override void writeJsonImpl(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Json serializing of type Vector3 not yet implemented.");
        }
    }
}
