using Microsoft.Win32;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClientApp.MVVM.View
{
    public partial class HandelImageView : UserControl
    {
        private string _columnName;
        private string _table;
        private Matrix _matrix = Matrix.Identity;
        private Point _lastMousePos;
        private bool _isDragging = false;

        public HandelImageView(string columnName)
        {
            InitializeComponent();

            string input = columnName;
            String[] parts = input.Split(';');
            _columnName = parts[0];
            _table = parts[1];

            Loaded += HandelImageView_Loaded;
            ResetTransform(); // Start clean
        }

        private void HandelImageView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImagesFromDatabase(_columnName, _table);
        }

        private void LoadImagesFromDatabase(string columnName, string tableName)
        {
            ImageWrapPanel.Children.Clear();
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ImageStorage.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            string query = $"SELECT [{columnName}] FROM [{tableName}] WHERE [{columnName}] IS NOT NULL";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] imageData = (byte[])reader[columnName];

                        BitmapImage image = new BitmapImage();
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                        }

                        // Create the image
                        Image img = new Image
                        {
                            Source = image,
                            Width = 150,
                            Height = 150,
                            Margin = new Thickness(5),
                            Cursor = Cursors.Hand,
                            Tag = false // Tag will store whether it's expanded or not
                        };

                        // Click to expand
                        img.MouseLeftButtonUp += (s, e) =>
                        {
                            // If overlay is already visible, do nothing
                            if (ImageOverlay.Visibility == Visibility.Visible && ImageOverlayBorder.Visibility == Visibility.Visible)
                                return;

                            ExpandedImage.Source = image;
                            ZoomScrollViewer.Focus();
                            ImageOverlay.Visibility = Visibility.Visible;
                            ImageOverlayBorder.Visibility = Visibility.Visible;
                        };

                        // 💡 Don't forget to add image to the panel
                        ImageWrapPanel.Children.Add(img);
                    }
                }
            }
        }

        private void ZoomScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ExpandedImage.Source == null) return;

            // Calculate zoom factor
            double zoom = e.Delta > 0 ? 1.1 : 0.9;
            double newScale = _matrix.M11 * zoom;

            // Prevent zooming out smaller than the original size
            if (newScale < 1)
                zoom = 1 / _matrix.M11;

            // Scale around mouse position
            Point mousePos = e.GetPosition(ExpandedImage);
            _matrix.ScaleAt(zoom, zoom, mousePos.X, mousePos.Y);

            ClampToBounds();
            MatrixTransform.Matrix = _matrix;
            e.Handled = true;
        }

        private void ClampToBounds()
        {
            double frameWidth = ZoomScrollViewer.ActualWidth;
            double frameHeight = ZoomScrollViewer.ActualHeight;

            double imageWidth = ExpandedImage.ActualWidth * _matrix.M11;
            double imageHeight = ExpandedImage.ActualHeight * _matrix.M22;

            double maxX = Math.Max(0, (imageWidth - frameWidth) / 2);
            double maxY = Math.Max(0, (imageHeight - frameHeight) / 2);

            if (_matrix.OffsetX > maxX) _matrix.OffsetX = maxX;
            if (_matrix.OffsetX < -maxX) _matrix.OffsetX = -maxX;
            if (_matrix.OffsetY > maxY) _matrix.OffsetY = maxY;
            if (_matrix.OffsetY < -maxY) _matrix.OffsetY = -maxY;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_matrix.M11 <= 1) return; // Disable dragging when not zoomed in
            _isDragging = true;
            _lastMousePos = e.GetPosition(ZoomScrollViewer);
            ExpandedImage.CaptureMouse();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ExpandedImage.ReleaseMouseCapture();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPos = e.GetPosition(ZoomScrollViewer);
                Vector delta = currentPos - _lastMousePos;
                _lastMousePos = currentPos;

                _matrix.Translate(delta.X, delta.Y);

                ClampToBounds();
                MatrixTransform.Matrix = _matrix;
            }
        }


        private void CloseOverlay_Click(object sender, RoutedEventArgs e)
        {
            ResetTransform();
            ExpandedImage.Source = null;
            ImageOverlay.Visibility = Visibility.Collapsed;
            ImageOverlayBorder.Visibility = Visibility.Collapsed;
        }

        private void ResetZoom()
        {
            _matrix = Matrix.Identity;               // Reset zoom & position
            MatrixTransform.Matrix = _matrix;        // Apply reset to image
            _isDragging = false;                     // Stop dragging
            ExpandedImage.Source = null;             // Clear image
        }

        private void ResetTransform()
        {
            _matrix = Matrix.Identity;
            MatrixTransform.Matrix = _matrix;
        }


        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    byte[] imageBytes = File.ReadAllBytes(filePath);
                    SaveImageToDatabase(_table, _columnName, imageBytes);
                }

                MessageBox.Show($"{openFileDialog.FileNames.Length} image(s) saved to database!");

                ImageWrapPanel.Children.Clear();
                LoadImagesFromDatabase(_columnName, _table);
            }
        }



        private void SaveImageToDatabase(string tableName, string columnName, byte[] data)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ImageStorage.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = $"INSERT INTO [{tableName}] ([{columnName}]) VALUES (@data)";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
