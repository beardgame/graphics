using System;
using System.IO;
using Newtonsoft.Json;
using OpenTK;

namespace amulware.Graphics.Serialization.JsonNet
{
    /// <summary>
    /// Json.NET converter for <see cref="Font"/>.
    /// </summary>
    internal sealed class FontConverter : JsonConverterBase<Font>
    {
        /// <summary>
        /// Reads a string from the reader, and converts it to a font.
        /// </summary>
        /// <param name="reader">The JSON reader to fetch data from.</param>
        /// <param name="serializer">The serializer for embedded serialization.</param>
        /// <returns>The <see cref="Font"/> identified in the JSON, or null.</returns>
        protected override Font readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            Vector2 pixelSymbolOffset = Vector2.Zero;
            Vector2 pixelSymbolSize = Vector2.One  * (1f / 16f);
            Vector2 symbolSize = Vector2.One;
            
            float[] letterWidths = null;

            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    break;

                var propertyName = (string)reader.Value;

                if (!reader.Read())
                    // no property value? stop reading and let JSON.NET fail
                    break;

                switch (propertyName)
                {
                    case "pixelSymbolOffset":
                        pixelSymbolOffset = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "pixelSymbolSize":
                        pixelSymbolSize = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "symbolSize":
                        symbolSize = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "letterWidths":
                        letterWidths = serializer.Deserialize<float[]>(reader);
                        break;
                    default:
                        throw new InvalidDataException(String.Format("Unknown property while desirialising font: {0}", propertyName));
                        break;
                }

            }
            
            
            return new Font(pixelSymbolOffset, pixelSymbolSize, symbolSize, letterWidths);
        }

        /// <summary>
        /// Converts the given <see cref="Font"/> to JSON.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="value">The value to convert</param>
        /// <param name="serializer">Unused by this serializer</param>
        protected override void writeJsonImpl(JsonWriter writer, Font value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Json serializing of type Font not yet implemented.");
        }
    }
}