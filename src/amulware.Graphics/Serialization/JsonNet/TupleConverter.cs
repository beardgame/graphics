using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace amulware.Graphics.Serialization.JsonNet
{
    internal class TupleConverter<T1, T2> : JsonConverterBase<Tuple<T1, T2>>
    {
        private readonly string item1Name;
        private readonly string item2Name;

        public TupleConverter(string item1Name, string item2Name)
        {
            this.item1Name = item1Name;
            this.item2Name = item2Name;
        }

        protected override Tuple<T1, T2> readJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            bool hasItem1 = false;
            bool hasItem2 = false;
            T1 item1 = default(T1);
            T2 item2 = default(T2);

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
                if (propertyName == this.item1Name)
                {
                    if (hasItem1)
                        throw new InvalidDataException("Multiple entries with same key in tuple!");
                    item1 = serializer.Deserialize<T1>(reader);
                    hasItem1 = true;
                }
                else if (propertyName == this.item2Name)
                {
                    if (hasItem2)
                        throw new InvalidDataException("Multiple entries with same key in tuple!");
                    item2 = serializer.Deserialize<T2>(reader);
                    hasItem2 = true;
                }
                else
                    throw new InvalidDataException(String.Format("Unknown property while deserialising tuple: {0}", propertyName));

            }

            if (!hasItem1 || !hasItem2)
                throw new InvalidDataException("Not enough entries in tuple!");


            return new Tuple<T1, T2>(item1, item2);
        }

        protected override void writeJsonImpl(JsonWriter writer, Tuple<T1, T2> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
