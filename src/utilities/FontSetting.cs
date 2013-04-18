using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AWGraphics
{
    public class FontSetting
    {

        private float[] letterWidth = new float[256];

        public FontSetting(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            for (int i = 0; i < 256; i++)
                letterWidth[i] = float.Parse(reader.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            reader.Close();
        }

        public float Width(int c)
        {
            return this.letterWidth[c];
        }
    }
}
