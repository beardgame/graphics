using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace amulware.Graphics.Serialization.JsonNet
{
    /// <summary>
    /// Container for all preconfigured Json.NET converters
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Converter for <see cref="Vector2"/>
        /// </summary>
        public static readonly JsonConverter Vector2Converter = new Vector2Converter();

        /// <summary>
        /// Converter for <see cref="Font"/>
        /// </summary>
        public static readonly JsonConverter FontConverter = new FontConverter();

        /// <summary>
        /// Converter for <see cref="UVRectangleContainer"/>
        /// </summary>
        public static readonly JsonConverter UVRectangleContainerConverter = new UVRectangleContainerConverter();

        /// <summary>
        /// Converter for <see cref="Color"/>
        /// </summary>
        public static readonly JsonConverter ColorContainerConverter = new ColorConverter();
    }
}
