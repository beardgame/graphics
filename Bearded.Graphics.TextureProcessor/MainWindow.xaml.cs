using System;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace Bearded.Graphics.TextureProcessor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            notify("Dropped files");

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;

                open(files[0]);
            }
        }

        private void open(string file)
        {
            FilenameDisplay.Text = file;

            Bitmap bitmap;

            try
            {
                bitmap = new Bitmap(file);
            }
            catch (Exception e)
            {
                notify($"Loading failed with {e.GetType().Name}: {e.Message}");
                return;
            }

            process(bitmap);
        }

        private void process(Bitmap bitmap)
        {
            notify($"Processing bitmap: {bitmap.Width}x{bitmap.Height}");

            var bitmaps = Processor
                .From(bitmap)
                .Process()
                .Select(b => b.ToSystemBitmap())
                .Select(b => new ImageData(b, "The Game"))
                .ToList();

            notify($"Finished processing bitmap: {bitmaps.Count} layers, {bitmap.Width}x{bitmap.Height}");

            ImageList.ItemsSource = bitmaps;
        }

        private void notify(string message)
        {
            NotificationDisplay.Text = message;
        }
    }
}
