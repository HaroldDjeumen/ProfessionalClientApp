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
using static System.Net.Mime.MediaTypeNames;

namespace ClientApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for RoomDetailView.xaml
    /// </summary>
    public partial class RoomDetailView : UserControl
    {
        private string room;
        public RoomDetailView()
        {
            
            InitializeComponent();
            Loaded += MyPage_Loaded;

        }

        private void MyPage_Loaded(object sender, RoutedEventArgs e)
        {
            room = currentRoomName.Text;
            LoadNotes(RoomListBox,room);
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

                    string query = $"SELECT Note FROM RoomNotes WHERE RoomName = @name";
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
