using ClientApp.Core;
using ClientApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

