using ClienApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RelayCommand PropertiesViewCommand { get; set; }
        public HandelPropertyViewModel HandelPropertyVM { get; set; }
        public VerdiPropertyViewModel VerdiPropertyVM { get; set; }
        public OlifantPropertyViewModel OlifantPropertyVM { get; set; }
        public ColeridgePropertyViewModel ColeridgePropertyVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public PropertiesViewModel PropertiesVM { get; set; }
        private object _currentView;
        private bool _canGoBack;

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
