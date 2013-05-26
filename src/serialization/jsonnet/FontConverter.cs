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
            Font.Builder builder = new Font.Builder();

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
                    case "uvSymbolOffset":
                        builder.UVSymbolOffset = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "uvSymbolSize":
                        builder.UVSymbolSize = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "symbolSize":
                        builder.SymbolSize = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "letterWidths":
                        builder.LetterWidths = serializer.Deserialize<float[]>(reader);
                        break;
                    default:
                        throw new InvalidDataException(String.Format("Unknown property while deserialising font: {0}", propertyName));
                }

            }


            return builder.Build();
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