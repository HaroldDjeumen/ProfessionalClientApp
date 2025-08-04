using ClientApp.Core;
using ClientApp.MVVM.Model;

namespace ClientApp.MVVM.ViewModel
{
    internal class WandEFolderViewModel : ObservableObject
    {
        private WandEFolderModel _currentProperty;

        public WandEFolderModel CurrentProperty
        {
            get { return _currentProperty; }
            set
            {
                _currentProperty = value;
                OnPropertyChanged();
            }
        }
    }
}
