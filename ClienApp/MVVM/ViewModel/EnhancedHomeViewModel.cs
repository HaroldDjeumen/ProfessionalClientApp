using ClientApp.Core;
using ClientApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace ClientApp.MVVM.ViewModel
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public object Data { get; set; }
    }

    public class PinnedNoteModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Order { get; set; }
    }

    public class RecentActivityModel
    {
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string TimeAgo => GetTimeAgo(Timestamp);

        private string GetTimeAgo(DateTime timestamp)
        {
            var timeSpan = DateTime.Now - timestamp;
            if (timeSpan.TotalMinutes < 1) return "Just now";
            if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}m ago";
            if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}h ago";
            return $"{(int)timeSpan.TotalDays}d ago";
        }
    }

    public class EnhancedHomeViewModel : ObservableObject
    {
        private string _searchText;
        private int _totalRooms;
        private int _occupiedRooms;
        private int _unpaidTenants;
        private int _todayCheckIns;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Implement search logic here
            }
        }

        public int TotalRooms
        {
            get => _totalRooms;
            set
            {
                _totalRooms = value;
                OnPropertyChanged();
            }
        }

        public int OccupiedRooms
        {
            get => _occupiedRooms;
            set
            {
                _occupiedRooms = value;
                OnPropertyChanged();
            }
        }

        public int UnpaidTenants
        {
            get => _unpaidTenants;
            set
            {
                _unpaidTenants = value;
                OnPropertyChanged();
            }
        }

        public int TodayCheckIns
        {
            get => _todayCheckIns;
            set
            {
                _todayCheckIns = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NotificationModel> Notifications { get; set; }
        public ObservableCollection<PinnedNoteModel> PinnedNotes { get; set; }
        public ObservableCollection<RecentActivityModel> RecentActivities { get; set; }

        // Commands
        public ICommand SearchCommand { get; set; }
        public ICommand PropertiesViewCommand { get; set; }
        public ICommand TenantsViewCommand { get; set; }
        public ICommand ReportsViewCommand { get; set; }
        public ICommand NoteViewCommand { get; set; }
        public ICommand PaymentsViewCommand { get; set; }
        public ICommand AddTenantCommand { get; set; }
        public ICommand CreateNoteCommand { get; set; }
        public ICommand MarkRentPaidCommand { get; set; }
        public ICommand GenerateReportCommand { get; set; }
        public ICommand NotificationClickCommand { get; set; }
        public ICommand AddPinnedNoteCommand { get; set; }

        public EnhancedHomeViewModel()
        {
            InitializeCollections();
            InitializeCommands();
            LoadDashboardData();
            LoadNotifications();
            LoadPinnedNotes();
            LoadRecentActivities();
        }

        private void InitializeCollections()
        {
            Notifications = new ObservableCollection<NotificationModel>();
            PinnedNotes = new ObservableCollection<PinnedNoteModel>();
            RecentActivities = new ObservableCollection<RecentActivityModel>();
        }

        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(ExecuteSearch);
            PropertiesViewCommand = new RelayCommand(o => NavigateToProperties());
            TenantsViewCommand = new RelayCommand(o => NavigateToTenants());
            ReportsViewCommand = new RelayCommand(o => NavigateToReports());
            NoteViewCommand = new RelayCommand(o => NavigateToNotes());
            PaymentsViewCommand = new RelayCommand(o => NavigateToPayments());
            AddTenantCommand = new RelayCommand(o => AddTenant());
            CreateNoteCommand = new RelayCommand(o => CreateNote());
            MarkRentPaidCommand = new RelayCommand(o => MarkRentPaid());
            GenerateReportCommand = new RelayCommand(o => GenerateReport());
            NotificationClickCommand = new RelayCommand(HandleNotificationClick);
            AddPinnedNoteCommand = new RelayCommand(o => AddPinnedNote());
        }

        private void LoadDashboardData()
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PropertyRooms.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Get total rooms
                    string totalRoomsQuery = "SELECT COUNT(*) FROM Rooms";
                    using (var command = new SQLiteCommand(totalRoomsQuery, connection))
                    {
                        TotalRooms = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // Get occupied rooms
                    string occupiedRoomsQuery = "SELECT COUNT(*) FROM Rooms WHERE Status = 'Occupied'";
                    using (var command = new SQLiteCommand(occupiedRoomsQuery, connection))
                    {
                        OccupiedRooms = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // Get unpaid tenants
                    string unpaidTenantsQuery = "SELECT COUNT(*) FROM Rooms WHERE RentPaidStatus = 'Unpaid' AND Status = 'Occupied'";
                    using (var command = new SQLiteCommand(unpaidTenantsQuery, connection))
                    {
                        UnpaidTenants = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // For now, set check-ins to 0 (can be enhanced later with actual BnB data)
                    TodayCheckIns = 0;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle database errors gracefully
                TotalRooms = 0;
                OccupiedRooms = 0;
                UnpaidTenants = 0;
                TodayCheckIns = 0;
            }
        }

        private void LoadNotifications()
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PropertyRooms.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Get overdue rent notifications
                    string overdueQuery = "SELECT RoomName, TenantName FROM Rooms WHERE RentPaidStatus = 'Unpaid' AND Status = 'Occupied'";
                    using (var command = new SQLiteCommand(overdueQuery, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Notifications.Add(new NotificationModel
                            {
                                Message = $"Rent overdue: {reader["TenantName"]} in {reader["RoomName"]}",
                                Type = "overdue",
                                Data = reader["RoomName"].ToString()
                            });
                        }
                    }

                    connection.Close();
                }

                // Add sample notifications if none exist
                if (Notifications.Count == 0)
                {
                    Notifications.Add(new NotificationModel
                    {
                        Message = "Welcome to your enhanced dashboard!",
                        Type = "info",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                Notifications.Add(new NotificationModel
                {
                    Message = "Unable to load notifications",
                    Type = "error",
                    Data = null
                });
            }
        }

        private void LoadPinnedNotes()
        {
            // Sample pinned notes (can be enhanced with database storage)
            PinnedNotes.Add(new PinnedNoteModel
            {
                Id = 1,
                Content = "Check water pressure in Building A",
                CreatedDate = DateTime.Now.AddDays(-1),
                Order = 1
            });

            PinnedNotes.Add(new PinnedNoteModel
            {
                Id = 2,
                Content = "Schedule maintenance for elevator",
                CreatedDate = DateTime.Now.AddDays(-2),
                Order = 2
            });
        }

        private void LoadRecentActivities()
        {
            // Sample recent activities (can be enhanced with actual activity tracking)
            RecentActivities.Add(new RecentActivityModel
            {
                Description = "Room 101 rent marked as paid",
                Timestamp = DateTime.Now.AddMinutes(-30)
            });

            RecentActivities.Add(new RecentActivityModel
            {
                Description = "New tenant added to Room 205",
                Timestamp = DateTime.Now.AddHours(-2)
            });

            RecentActivities.Add(new RecentActivityModel
            {
                Description = "Note created for maintenance request",
                Timestamp = DateTime.Now.AddHours(-4)
            });

            RecentActivities.Add(new RecentActivityModel
            {
                Description = "Monthly report generated",
                Timestamp = DateTime.Now.AddDays(-1)
            });

            RecentActivities.Add(new RecentActivityModel
            {
                Description = "Room 303 marked as vacant",
                Timestamp = DateTime.Now.AddDays(-2)
            });
        }

        private void ExecuteSearch(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            // Implement comprehensive search logic here
            // This could search through rooms, tenants, notes, payments, etc.
        }

        private void NavigateToProperties()
        {
            // Navigate to properties view
        }

        private void NavigateToTenants()
        {
            // Navigate to tenants view
        }

        private void NavigateToReports()
        {
            // Navigate to reports view
        }

        private void NavigateToNotes()
        {
            // Navigate to notes view
        }

        private void NavigateToPayments()
        {
            // Navigate to payments view
        }

        private void AddTenant()
        {
            // Open add tenant dialog/form
        }

        private void CreateNote()
        {
            // Open create note dialog/form
        }

        private void MarkRentPaid()
        {
            // Open mark rent paid dialog/form
        }

        private void GenerateReport()
        {
            // Generate and display report
        }

        private void HandleNotificationClick(object parameter)
        {
            if (parameter is NotificationModel notification)
            {
                // Handle notification click based on type
                switch (notification.Type)
                {
                    case "overdue":
                        // Navigate to specific room/tenant
                        break;
                    case "checkout":
                        // Navigate to checkout process
                        break;
                    case "maintenance":
                        // Navigate to maintenance requests
                        break;
                }
            }
        }

        private void AddPinnedNote()
        {
            // Open dialog to add new pinned note
            var newNote = new PinnedNoteModel
            {
                Id = PinnedNotes.Count + 1,
                Content = "New reminder...",
                CreatedDate = DateTime.Now,
                Order = PinnedNotes.Count + 1
            };
            PinnedNotes.Add(newNote);
        }
    }
}

