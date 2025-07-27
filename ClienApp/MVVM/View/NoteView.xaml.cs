using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClientApp.MVVM.View
{
    public partial class NoteView : UserControl
    {
        public NoteView()
        {
            InitializeComponent();
            LoadRoomNames();
        }

        private void PropertiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PropertiesComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedProperty = selectedItem.Content.ToString();
                LoadNotes(PropertyListBox, selectedProperty, "PropertyNotes", "PropertyName");
            }
        }

        private void OtherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OtherComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedOther = selectedItem.Content.ToString();
                LoadNotes(OtherListBox, selectedOther, "OtherNotes", "OtherName");
            }
        }

        private void RoomsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedRoom = selectedItem.Content.ToString();
                LoadNotes(RoomListBox, selectedRoom, "RoomNotes", "RoomName");
            }
            else if (RoomsComboBox.SelectedItem is string selectedRoom)
            {
                LoadNotes(RoomListBox, selectedRoom, "RoomNotes", "RoomName");
            }
        }


        private void LoadNotes(ListBox listBox, string name, string table, string nameField)
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Notes.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                listBox.Items.Clear();

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT Note FROM {table} WHERE {nameField} = @name";
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

        private void LoadRoomNames()
        {
            try
            {
                ComboBox roomsComboBox = FindRoomsComboBox();
                if (roomsComboBox == null) return;

                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PropertyRooms.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT RoomName FROM Rooms";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string roomName = reader["RoomName"].ToString();
                            roomsComboBox.Items.Add(roomName);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading room names: {ex.Message}");
            }
        }
         
        private ComboBox FindRoomsComboBox()
        {
            foreach (var child in Rooms.Children)
            {
                if (child is ComboBox cb)
                    return cb;
            }
            return null;
        }

        private void AddButton_clicked(object sender, RoutedEventArgs e)
        {
            if (sender == AddNote)
                AddOrUpdateNote(PropertiesComboBox, PropertyListBox, "PropertyNotes", "PropertyName");
            else if (sender == AddNote2)
                AddOrUpdateNote(RoomsComboBox, RoomListBox, "RoomNotes", "RoomName");
            else if (sender == AddNote3)
                AddOrUpdateNote(OtherComboBox, OtherListBox, "OtherNotes", "OtherName");
        }

        private void AddOrUpdateNote(ComboBox comboBox, ListBox listBox, string table, string nameField)
        {
            string selected = comboBox.SelectedItem is ComboBoxItem item ? item.Content.ToString() : comboBox.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selected))
            {
                MessageBox.Show("Please select a property or room.");
                return;
            }

            TextBox inputBox = new TextBox
            {
                Margin = new Thickness(10),
                FontSize = 16,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 200,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            Button saveButton = new Button
            {
                Content = "Save Note",
                Margin = new Thickness(10),
                Padding = new Thickness(8),
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = $"Enter note for {selected}:",
                Margin = new Thickness(10),
                FontSize = 16
            });
            panel.Children.Add(inputBox);
            panel.Children.Add(saveButton);

            Window popup = new Window
            {
                Title = "Add Note",
                Width = 400,
                Height = 350,
                Content = panel,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.White
            };

            saveButton.Click += (s, e) =>
            {
                string newNote = inputBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(newNote))
                {
                    MessageBox.Show("Note cannot be empty.");
                    return;
                }

                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Notes.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO {table} ({nameField}, Note) VALUES (@name, @note)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", selected);
                        command.Parameters.AddWithValue("@note", newNote);
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                listBox.Items.Add(newNote);
                MessageBox.Show("Note saved successfully.");
                popup.Close();
            };

            popup.ShowDialog();
        }

        private void RemoveButton_clicked(object sender, RoutedEventArgs e)
        {
            if (sender == RemoveNotes)
                RemoveNote(PropertiesComboBox, PropertyListBox, "PropertyNotes", "PropertyName");
            else if (sender == RemoveNotes2)
                RemoveNote(RoomsComboBox, RoomListBox, "RoomNotes", "RoomName");
            else if (sender == RemoveNotes3)
                RemoveNote(OtherComboBox, OtherListBox, "OtherNotes", "OtherName");
        }

        private void RemoveNote(ComboBox comboBox, ListBox listBox, string table, string nameField)
        {
            string selected = comboBox.SelectedItem is ComboBoxItem item ? item.Content.ToString() : comboBox.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selected))
            {
                MessageBox.Show("Please select a property or room.");
                return;
            }

            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a note to delete.");
                return;
            }

            string selectedNote = listBox.SelectedItem.ToString();

            var result = MessageBox.Show($"Are you sure you want to delete this note?\n\n\"{selectedNote}\"", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Notes.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"DELETE FROM {table} WHERE {nameField} = @name AND Note = @note";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", selected);
                    command.Parameters.AddWithValue("@note", selectedNote);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            listBox.Items.Remove(selectedNote);
            MessageBox.Show("Note deleted.");
        }
    }

}