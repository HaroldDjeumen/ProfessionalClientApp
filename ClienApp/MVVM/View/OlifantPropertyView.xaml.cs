using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for OlifantPropertyView.xaml
    /// </summary>
    public partial class OlifantPropertyView : UserControl
    {
        private string roomname;
        public OlifantPropertyView()
        {
            InitializeComponent();
            LoadNotes(PropertyListBox, "Sasol");
        }

        private void LoadRoomFromDatabase(string roomName)
        {
            // Clear previous info
            RoomInfo.Children.Clear();

            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PropertyRooms.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            string query = "SELECT * FROM Rooms WHERE RoomName = @RoomName";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomName", roomName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get room info
                            string name = reader["RoomName"].ToString();
                            string tenantname = reader["TenantName"].ToString();
                            string rentPaidStatus = reader["RentPaidstatus"].ToString();
                            string status = reader["Status"].ToString();
                            string stayPeriod = reader["StayPeriod"].ToString();
                            string roomType = reader["RoomType"].ToString();

                            // Create main grid with 3 columns
                            var mainGrid = new Grid { Margin = new Thickness(5, 5, 20, 5) };
                            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Room Name
                            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });                     // Separator Line
                            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });  // Details

                            // Left: Room Name
                            var roomNameBlock = new TextBlock
                            {
                                Text = name,
                                Foreground = Brushes.White,
                                FontSize = 20,
                                FontWeight = FontWeights.Bold,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                TextWrapping = TextWrapping.Wrap
                            };
                            Grid.SetColumn(roomNameBlock, 0);

                            // Separator Line
                            var separator = new Border
                            {
                                Background = Brushes.Gray,
                                Width = 1,
                                Margin = new Thickness(0, 0, 0, 0)
                            };
                            Grid.SetColumn(separator, 1);

                            // Right: Details
                            var detailsPanel = new StackPanel { Margin = new Thickness(25, 0, 0, 0) };
                            detailsPanel.Children.Add(CreateInfoRow("Tenant", tenantname));
                            detailsPanel.Children.Add(CreateInfoRow("Rent", rentPaidStatus));
                            detailsPanel.Children.Add(CreateInfoRow("Status", status));
                            detailsPanel.Children.Add(CreateInfoRow("Stay Period", stayPeriod));
                            detailsPanel.Children.Add(CreateInfoRow("Room Type", roomType));
                            Grid.SetColumn(detailsPanel, 2);

                            // Add to grid
                            mainGrid.Children.Add(roomNameBlock);
                            mainGrid.Children.Add(separator);
                            mainGrid.Children.Add(detailsPanel);

                            // Add grid to RoomInfo
                            RoomInfo.Children.Add(mainGrid);

                        }
                    }
                }
            }
        }

        // Helper to create a row of details
        private Grid CreateInfoRow(string label, string value)
        {
            var grid = new Grid { Margin = new Thickness(0, 2, 0, 2) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var labelBlock = new TextBlock
            {
                Text = label + " : ",
                Foreground = Brushes.LightGray,
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(labelBlock, 0);

            var valueBlock = new TextBlock
            {
                Text = value,
                Foreground = Brushes.White,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(valueBlock, 1);

            grid.Children.Add(labelBlock);
            grid.Children.Add(valueBlock);

            return grid;
        }

        private void Sasol1_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 1";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol2_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 2";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol3_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 3";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol4_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 4";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol5_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 5";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol6_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 6";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol7_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 7";
            LoadRoomFromDatabase(roomname);
        }

        private void Sasol8_clicked(object sender, RoutedEventArgs e)
        {
            roomname = "Sasol 8";
            LoadRoomFromDatabase(roomname);
        }

        private void LoadNotes(ListBox listBox, string name)
        {
            try
            {
                string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Notes.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                listBox.Items.Clear();

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT Note FROM PropertyNotes WHERE PropertyName = @name";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string note = reader["Note"].ToString();
                                listBox.Items.Add(note);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading notes: {ex.Message}");
            }
        }
    }
}
