using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace amulware.Graphics.Serialization.JsonNet
{
    /// <summary>
    /// Static class containing extensions to configure Json.NET for converting our types.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Configures Json.NET with all converters required to serialize or deserialze Graphics related data types
        /// </summary>
        public static JsonSerializerSettings ConfigureForGraphics(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(Converters.Vector2Converter);
            settings.Converters.Add(Converters.Vector3Converter);
            settings.Converters.Add(Converters.FontConverter);
            settings.Converters.Add(Converters.UVRectangleContainerConverter);
            settings.Converters.Add(Converters.ColorContainerConverter);

            // return to allow for chaining
            return settings;
        }

        /// <summary>
        /// Configures Json.NET with all converters required to serialize or deserialze Graphics related data types
        /// </summary>
        public static JsonSerializer ConfigureForGraphics(this JsonSerializer serializer)
        {
            serializer.Converters.Add(Converters.Vector2Converter);
            serializer.Converters.Add(Converters.Vector3Converter);
            serializer.Converters.Add(Converters.FontConverter);
            serializer.Converters.Add(Converters.UVRectangleContainerConverter);
            serializer.Converters.Add(Converters.ColorContainerConverter);

            // return to allow for chaining
            return serializer;
        }
    }
}
