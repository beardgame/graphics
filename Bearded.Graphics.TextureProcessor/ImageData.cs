using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Bearded.Graphics.TextureProcessor
{
    public sealed class ImageData
    {
        public BitmapImage Image { get; }
        public string Caption { get; }

        public ImageData(Bitmap bitmap, string caption)
        {
            Image = ToBitmapImage(bitmap);
            Caption = caption;
        }

        private static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}
