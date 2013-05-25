using System;
using System.IO;
using Newtonsoft.Json;
using OpenTK;

namespace amulware.Graphics.Serialization.JsonNet
{
    /// <summary>
    /// Json.NET converter for <see cref="Vector2"/>.
    /// </summary>
    internal sealed class Vector2Converter : JsonConverterBase<Vector2>
    {
        /// <summary>
        /// Reads a string from the reader, and converts it to a Vector2.
        /// </summary>
        /// <param name="reader">The JSON reader to fetch data from.</param>
        /// <param name="serializer">The serializer for embedded serialization.</param>
        /// <returns>The <see cref="Vector2"/> identified in the JSON, or null.</returns>
        protected override Vector2 readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            if (reader.Read() && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
            {
                float x = Convert.ToSingle(reader.Value);
                if (reader.Read() && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                {
                    float y = Convert.ToSingle(reader.Value);
                    if (reader.Read() && reader.TokenType == JsonToken.EndArray)
                        return new Vector2(x, y);
                }
            }
            throw new InvalidDataException("A Vector2 must be an array of two numbers.");
        }

        /// <summary>
        /// Converts the given <see cref="Vector2"/> to JSON.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="value">The value to convert</param>
        /// <param name="serializer">Unused by this serializer</param>
        protected override void writeJsonImpl(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Json serializing of type Vector2 not yet implemented.");
        }
    }
}