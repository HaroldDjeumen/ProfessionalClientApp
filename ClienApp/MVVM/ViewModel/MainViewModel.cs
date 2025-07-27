using ClientApp.Core;
using ClientApp.MVVM.Model;
using ClientApp.MVVM.ViewModel;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ClientApp.MVVM.ViewModel
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
        public RelayCommand ColeridgeImageViewCommand {get; set;}

        public RelayCommand VerdiImageViewCommand { get; set; }
        public RelayCommand OlifantImageViewCommand { get; set; }

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

        public ColeridgeImageViewModel ColeridgeImageVM { get; set; }
        public HandelImageViewModel HandelImageVM { get; set; }
        public VerdiImageViewModel VerdiImageVM { get; set; }
        public OlifantImageViewModel OlifantImageVM { get; set; }

        public ICommand HandelImageViewCommand { get; set; }

        private object _currentView;
        private bool _canGoBack;

        private string _selectedHandelColumn;

        public string SelectedHandelColumn
        {
            get => _selectedHandelColumn;
            set
            {
                _selectedHandelColumn = value;
                OnPropertyChanged(nameof(SelectedHandelColumn));
            }
        }

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
            ColeridgeImageVM = new ColeridgeImageViewModel();
            VerdiImageVM = new VerdiImageViewModel();
            OlifantImageVM = new OlifantImageViewModel();

            // Default view
            CurrentView = HomeVM;
            // Commands
            HomeViewCommand = new RelayCommand(o => CurrentView = HomeVM);
            PropertiesViewCommand = new RelayCommand(o => CurrentView = PropertiesVM);
            GallaryViewCommand = new RelayCommand(o => CurrentView = GallaryVM);
            ColeridgeImageViewCommand = new RelayCommand(o => CurrentView = ColeridgeImageVM);
            VerdiImageViewCommand = new RelayCommand(o => CurrentView = VerdiImageVM);
            OlifantImageViewCommand = new RelayCommand(o => CurrentView = OlifantImageVM);
            NoteViewCommand = new RelayCommand(o => CurrentView = NoteVM);
            HandelPropertyViewCommand = new RelayCommand(o => CurrentView = HandelPropertyVM);
            VerdiPropertyViewCommand = new RelayCommand(o => CurrentView = VerdiPropertyVM);
            OlifantPropertyViewCommand = new RelayCommand(o => CurrentView = OlifantPropertyVM);
            ColeridgePropertyViewCommand = new RelayCommand(o => CurrentView = ColeridgePropertyVM);

            HandelImageViewCommand = new RelayCommand(o =>
            {
                string selectedFolder = o as string;
                if (!string.IsNullOrEmpty(selectedFolder))
                {
                    HandelImageVM = new HandelImageViewModel(selectedFolder);
                    CurrentView = HandelImageVM;
                }
            });

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

