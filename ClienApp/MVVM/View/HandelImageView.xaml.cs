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

        public HandelImageView(string columnName)
        {
            InitializeComponent();
            _columnName = columnName;

            Loaded += HandelImageView_Loaded;
        }

        private void HandelImageView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImagesFromDatabase(_columnName);
        }

        private void LoadImagesFromDatabase(string columnName)
        {
            ImageWrapPanel.Children.Clear();
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ImageStorage.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            string query = $"SELECT [{columnName}] FROM HandelImage WHERE [{columnName}] IS NOT NULL";

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
                            if (ImageOverlay.Visibility == Visibility.Visible)
                                return;

                            ExpandedImage.Source = image;
                            ZoomScrollViewer.Focus();
                            ImageOverlay.Visibility = Visibility.Visible;
                        };

                        // 💡 Don't forget to add image to the panel
                        ImageWrapPanel.Children.Add(img);
                    }
                }
            }
        }

        private void ZoomScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                double zoom = e.Delta > 0 ? 0.1 : -0.1;
                ZoomTransform.ScaleX += zoom;
                ZoomTransform.ScaleY += zoom;

                if (ZoomTransform.ScaleX < 1) ZoomTransform.ScaleX = 1;
                if (ZoomTransform.ScaleY < 1) ZoomTransform.ScaleY = 1;

                e.Handled = true;
            }
        }


        private void CloseOverlay_Click(object sender, RoutedEventArgs e)
        {
            ZoomTransform.ScaleX = 1;
            ZoomTransform.ScaleY = 1;

            ImageOverlay.Visibility = Visibility.Collapsed;
            ExpandedImage.Source = null;
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
            openFileDialog.Multiselect = true; // Allow multiple image selection

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    byte[] imageBytes = File.ReadAllBytes(filePath);
                    SaveImageToDatabase(_columnName, imageBytes);
                }

                MessageBox.Show($"{openFileDialog.FileNames.Length} image(s) saved to database!");

                ImageWrapPanel.Children.Clear();
                LoadImagesFromDatabase(_columnName);
            }
        }



        private void SaveImageToDatabase(string columnName, byte[] data)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ImageStorage.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = $"INSERT INTO HandelImage ([{columnName}]) VALUES (@data)";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
