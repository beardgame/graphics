using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bearded.Graphics.TextureProcessor
{
    public partial class MainWindow
    {
        private string lastFile;

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
            lastFile = file;

            using var bitmap = new Bitmap(file);
            process(bitmap);
        }

        private void process(Bitmap bitmap)
        {
            notify($"Processing bitmap: {bitmap.Width}x{bitmap.Height}");

            var bitmaps = Processor
                .From(bitmap)
                .Process()
                .Select(b => new ImageData(b.Bitmap, $"{b.Name} ({b.Time.TotalSeconds:0.00}s)"))
                .ToList();

            notify($"Finished processing bitmap: {bitmaps.Count} layers, {bitmap.Width}x{bitmap.Height}");

            ImageList.ItemsSource = bitmaps;
        }

        private void notify(string message)
        {
            NotificationDisplay.Text = message;
        }

        private void scrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + e.Delta);
            e.Handled = true;
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            open(lastFile);
        }
    }
}
