using ClienApp.Core;
using ClienApp.MVVM.Model;
using ClienApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienApp.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private Stack<object> _viewHistory = new Stack<object>();
        public RelayCommand BackCommand { get; set; }
        public RelayCommand HandelPropertyViewCommand { get; set; }
        public RelayCommand VerdiPropertyViewCommand { get; set; }
        public RelayCommand OlifantPropertyViewCommand { get; set; }
        public RelayCommand ColeridgePropertyViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand RoomDetailViewCommand { get; set; }
        public RelayCommand PropertiesViewCommand { get; set; }
        public HandelPropertyViewModel HandelPropertyVM { get; set; }
        public VerdiPropertyViewModel VerdiPropertyVM { get; set; }
        public OlifantPropertyViewModel OlifantPropertyVM { get; set; }
        public ColeridgePropertyViewModel ColeridgePropertyVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public RoomDetailViewModel RoomDetailVM { get; set; }
        public PropertiesViewModel PropertiesVM { get; set; }
        private object _currentView;
        private bool _canGoBack;

        private List<RoomDetailModel> Rooms = new List<RoomDetailModel>
        {
           new RoomDetailModel {Id = 1, RoomName = "Handel 1", TenantName = "John", RentPaidStatus = "Paid",  Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0"},
           new RoomDetailModel {Id = 2, RoomName = "Handel 2", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },
           new RoomDetailModel {Id = 3, RoomName = "Handel 3", TenantName = "Mike", RentPaidStatus = "Paid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },
           new RoomDetailModel {Id = 4, RoomName = "Handel 4", TenantName = "John", RentPaidStatus = "Paid",  Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0"},
           new RoomDetailModel {Id = 5, RoomName = "Handel 5", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },
           new RoomDetailModel {Id = 6, RoomName = "Handel 6", TenantName = "Mike", RentPaidStatus = "Paid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },
           new RoomDetailModel {Id = 7, RoomName = "Handel 7", TenantName = "John", RentPaidStatus = "Paid",  Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0"},
           new RoomDetailModel {Id = 8, RoomName = "Handel 8", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },
           new RoomDetailModel {Id = 9, RoomName = "Handel 9", TenantName = "John", RentPaidStatus = "Paid",  Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0"},
           new RoomDetailModel {Id = 10, RoomName = "Handel 10", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },
           new RoomDetailModel {Id = 11, RoomName = "Handel 11", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#5D2A29", Lightcolor = "#EFC0B0" },

           new RoomDetailModel {Id = 12, RoomName = "Verdi 1", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 13, RoomName = "Verdi 2", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 14, RoomName = "Verdi 3", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 15, RoomName = "Verdi 4", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 16, RoomName = "Verdi 5", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 17, RoomName = "Verdi 6", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 18, RoomName = "Verdi 7", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },
           new RoomDetailModel {Id = 19, RoomName = "Verdi 8", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#222C21", Lightcolor = "#AFADA9" },

           new RoomDetailModel {Id = 20, RoomName = "Sasol 1", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 21, RoomName = "Sasol 2", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 22, RoomName = "Sasol 3", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 23, RoomName = "Sasol 4", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 24, RoomName = "Sasol 5", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 25, RoomName = "Sasol 6", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 26, RoomName = "Sasol 7", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },
           new RoomDetailModel {Id = 27, RoomName = "Sasol 8", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#7B6152", Lightcolor = "#E2CEB5" },

           new RoomDetailModel {Id = 28, RoomName = "VaalM 1", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 29, RoomName = "VaalM 2", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 30, RoomName = "VaalM 3", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 31, RoomName = "VaalM 4", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 32, RoomName = "VaalM 5", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 33, RoomName = "VaalM 6", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 34, RoomName = "VaalM 7", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 35, RoomName = "VaalM 8", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 36, RoomName = "VaalM 9", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" },
           new RoomDetailModel {Id = 37, RoomName = "VaalM 10", TenantName = "Sarah", RentPaidStatus = "Unpaid", Status = "Available", StayPeriod = "3 Months", RoomImage = "/Images/Handel-St.jpg", RoomType = "Rental", Darkcolor = "#B3845A", Lightcolor = "#E0CFC5" }
          
        };


        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                _canGoBack = value;
                OnPropertyChanged();
            }
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != null && _currentView != value)
                    _viewHistory.Push(_currentView);

                _currentView = value;
                OnPropertyChanged();
                CanGoBack = _viewHistory.Count > 0;
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            PropertiesVM = new PropertiesViewModel();
            HandelPropertyVM = new HandelPropertyViewModel();
            VerdiPropertyVM = new VerdiPropertyViewModel();
            OlifantPropertyVM = new OlifantPropertyViewModel();
            ColeridgePropertyVM = new ColeridgePropertyViewModel();
            RoomDetailVM = new RoomDetailViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            PropertiesViewCommand = new RelayCommand(o =>
            {
                CurrentView = PropertiesVM;
            });

            HandelPropertyViewCommand = new RelayCommand(o =>
            {
                CurrentView = HandelPropertyVM;
            });

            VerdiPropertyViewCommand = new RelayCommand(o =>
            {
                CurrentView = VerdiPropertyVM;
            });

            OlifantPropertyViewCommand = new RelayCommand(o =>
            {
                CurrentView = OlifantPropertyVM;
            });

            ColeridgePropertyViewCommand = new RelayCommand(o =>
            {
                CurrentView = ColeridgePropertyVM;
            });

            RoomDetailViewCommand = new RelayCommand(o =>
            {
                if (o is string roomName)
                {
                    var matchedRoom = Rooms.FirstOrDefault(r => r.RoomName == roomName);
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

            RoomDetailVM = new RoomDetailViewModel();

        }

       


    }
}
