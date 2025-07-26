using ClienApp.Core;
using ClienApp.MVVM.Model;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace ClienApp.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private Stack<object> _viewHistory = new Stack<object>();

        // Commands
        public RelayCommand BackCommand { get; set; }
        public RelayCommand HandelPropertyViewCommand { get; set; }
        public RelayCommand VerdiPropertyViewCommand { get; set; }
        public RelayCommand OlifantPropertyViewCommand { get; set; }
        public RelayCommand ColeridgePropertyViewCommand { get; set; }
        public RelayCommand GallaryViewCommand { get; set; }
        public RelayCommand NoteViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand RoomDetailViewCommand { get; set; }
        public RelayCommand PropertiesViewCommand { get; set; }

        // ViewModels
        public HandelPropertyViewModel HandelPropertyVM { get; set; }
        public VerdiPropertyViewModel VerdiPropertyVM { get; set; }
        public OlifantPropertyViewModel OlifantPropertyVM { get; set; }
        public ColeridgePropertyViewModel ColeridgePropertyVM { get; set; }
        public GallaryViewModel GallaryVM { get; set; }
        public NoteViewModel NoteVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public RoomDetailViewModel RoomDetailVM { get; set; }
        public PropertiesViewModel PropertiesVM { get; set; }

        private object _currentView;
        private bool _canGoBack;

        public bool CanGoBack
        {
            get => _canGoBack;
            set
            {
                _canGoBack = value;
                OnPropertyChanged();
            }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != null && _currentView != value)
                    _viewHistory.Push(_currentView);

                _currentView = value;
                OnPropertyChanged();
                CanGoBack = _viewHistory.Count > 0;
            }
        }
        private List<RoomDetailModel> LoadRoomsFromDatabase()
        {
            List<RoomDetailModel> loadedRooms = new List<RoomDetailModel>();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PropertyRooms.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Rooms";
                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var room = new RoomDetailModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            RoomName = reader["RoomName"].ToString(),
                            TenantName = reader["TenantName"].ToString(),
                            RentPaidStatus = reader["RentPaidStatus"].ToString(),
                            Status = reader["Status"].ToString(),
                            StayPeriod = reader["StayPeriod"].ToString(),
                            RoomImage = reader["RoomImage"].ToString(),
                            RoomType = reader["RoomType"].ToString(),
                            Darkcolor = reader["Darkcolor"].ToString(),
                            Lightcolor = reader["Lightcolor"].ToString()
                        };
                        loadedRooms.Add(room);
                    }
                }

                connection.Close();
            }

            return loadedRooms;
        }



        public MainViewModel()

        {
            // Initialize view models
            HomeVM = new HomeViewModel();
            PropertiesVM = new PropertiesViewModel();
            GallaryVM = new GallaryViewModel();
            NoteVM = new NoteViewModel();
            HandelPropertyVM = new HandelPropertyViewModel();
            VerdiPropertyVM = new VerdiPropertyViewModel();
            OlifantPropertyVM = new OlifantPropertyViewModel();
            ColeridgePropertyVM = new ColeridgePropertyViewModel();
            RoomDetailVM = new RoomDetailViewModel();

            // Default view
            CurrentView = HomeVM;
            // Commands
            HomeViewCommand = new RelayCommand(o => CurrentView = HomeVM);
            PropertiesViewCommand = new RelayCommand(o => CurrentView = PropertiesVM);
            GallaryViewCommand = new RelayCommand(o => CurrentView = GallaryVM);
            NoteViewCommand = new RelayCommand(o => CurrentView = NoteVM);
            HandelPropertyViewCommand = new RelayCommand(o => CurrentView = HandelPropertyVM);
            VerdiPropertyViewCommand = new RelayCommand(o => CurrentView = VerdiPropertyVM);
            OlifantPropertyViewCommand = new RelayCommand(o => CurrentView = OlifantPropertyVM);
            ColeridgePropertyViewCommand = new RelayCommand(o => CurrentView = ColeridgePropertyVM);
            RoomDetailViewCommand = new RelayCommand(o =>
            {
                if (o is string roomName)
                {
                    var matchedRoom = LoadRoomsFromDatabase().FirstOrDefault(r => r.RoomName == roomName);

                    if (matchedRoom != null)
                    {
                        RoomDetailVM.CurrentRoom = matchedRoom;
                        CurrentView = RoomDetailVM;
                    }
                    else
                    {
                        MessageBox.Show("Room not found!");
                    }
                }
            });

            BackCommand = new RelayCommand(o =>
            {
                if (_viewHistory.Count > 0)
                {
                    _currentView = _viewHistory.Pop();
                    OnPropertyChanged(nameof(CurrentView));
                    CanGoBack = _viewHistory.Count > 0;
                }
            }, o => CanGoBack);

          
        }
    }
}

