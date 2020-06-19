using Newtonsoft.Json;
using System;
using System.IO;

namespace amulware.Graphics.Serialization.JsonNet
{
    internal class ColorConverter : JsonConverterBase<Color>
    {

        protected override Color readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var s = (string)reader.Value;

                foreach (var field in typeof(Color).GetFields())
                {
                    if (field.IsStatic && field.FieldType == typeof(Color)
                        && field.Name.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                        return (Color)field.GetValue(null);
                }

                try
                {
                    return new Color(Convert.ToUInt32(s, 16));
                }
                catch(Exception)
                {
                    throw new InvalidDataException("Color has unknown or invalid string value.");
                }
            }
            if (reader.TokenType == JsonToken.StartArray)
            {
                if (reader.Read() && reader.TokenType == JsonToken.Integer)
                {
                    byte r = Convert.ToByte(reader.Value);

                    if (reader.Read() && reader.TokenType == JsonToken.Integer)
                    {
                        byte g = Convert.ToByte(reader.Value);

                        if (reader.Read() && reader.TokenType == JsonToken.Integer)
                        {
                            byte b = Convert.ToByte(reader.Value);

                            if (reader.Read())
                            {
                                if (reader.TokenType == JsonToken.Integer)
                                {
                                    byte a = Convert.ToByte(reader.Value);
                                    if (reader.Read() && reader.TokenType == JsonToken.EndArray)
                                        return new Color(r, g, b, a);
                                }

                                if (reader.TokenType == JsonToken.EndArray)
                                {
                                    return new Color(r, g, b);
                                }
                            }
                        }
                    }
                }
            }

            throw new InvalidDataException("Colour has no or invalid value.");
        }

        protected override void writeJsonImpl(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
