using ClientApp.Core;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ClientApp.MVVM.ViewModel
{
    public class HandelImageViewModel : ObservableObject
    {
        private string _selectedColumn;
        public string SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                _selectedColumn = value;
                OnPropertyChanged();
            }
        }
    }
}
