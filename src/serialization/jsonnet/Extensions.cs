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
        public static JsonSerializerSettings ConfigureForGraphics(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(Converters.Vector2Converter);
            settings.Converters.Add(Converters.FontConverter);

            // return to allow for chaining
            return settings;
        }
    }
}
