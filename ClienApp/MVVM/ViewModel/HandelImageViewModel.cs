using ClientApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public HandelImageViewModel(string selectedColumn)
        {
            SelectedColumn = selectedColumn;
            LoadHandelFromDataBase(selectedColumn);
        }

        private void LoadHandelFromDataBase(string columnName)
        {
            // your logic here
        }
    }
}
