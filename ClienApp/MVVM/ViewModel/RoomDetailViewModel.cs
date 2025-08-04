using ClientApp.Core;
using ClientApp.MVVM.Model;

namespace ClientApp.MVVM.ViewModel
{
    internal class RoomDetailViewModel : ObservableObject
    {
        private RoomDetailModel _currentRoom;

        public RoomDetailModel CurrentRoom
        {
            get { return _currentRoom; }
            set
            {
                _currentRoom = value;
                OnPropertyChanged();
            }
        }
    }
}

