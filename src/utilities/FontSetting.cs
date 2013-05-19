using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    /// <summary>
    /// Settings container for fonts, containing information about
    /// </summary>
    public class FontSetting
    {

        private float[] letterWidth = new float[256];

        /// <summary>
        /// Initializes a new instance of the <see cref="FontSetting"/> class.
        /// </summary>
        /// <param name="filename">Path to the filename to load the settings from</param>
        public FontSetting(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            for (int i = 0; i < 256; i++)
                letterWidth[i] = float.Parse(reader.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            reader.Close();
        }

        /// <summary>
        /// Returns the width of an ASCII character
        /// </summary>
        /// <param name="c">ASCII character</param>
        /// <returns>character's width</returns>
        public float Width(int c)
        {
            return this.letterWidth[c];
        }
    }
}
