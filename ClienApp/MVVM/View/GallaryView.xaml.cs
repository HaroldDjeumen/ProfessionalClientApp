using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for GallaryView.xaml
    /// </summary>
    public partial class GallaryView : UserControl
    {
        public GallaryView()
        {
            InitializeComponent();
        }

        private void Handel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Verdi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Sasol_Click(object sender, RoutedEventArgs e)
        {
            // optional
        }

        private void VaalMall_Click(object sender, RoutedEventArgs e)
        {
            // optional
        }
        private void Properties_Click(object sender, RoutedEventArgs e)
        {
            // your code here (optional)
        }

        private void HomeHandel_Click(object sender, RoutedEventArgs e)
        {
            Handel.Visibility = Visibility.Visible;
            Sasol.Visibility = Visibility.Collapsed;
            Verdi.Visibility = Visibility.Collapsed;
            VaalMall.Visibility = Visibility.Collapsed;
            Properties.Visibility = Visibility.Collapsed;
        }

        private void HomeSasol_Click(object sender, RoutedEventArgs e)
        {
            Handel.Visibility = Visibility.Collapsed;
            Sasol.Visibility = Visibility.Visible;
            Verdi.Visibility = Visibility.Collapsed;
            VaalMall.Visibility = Visibility.Collapsed;
            Properties.Visibility = Visibility.Collapsed;

        }

        private void HomeVaalMall_Click(object sender, RoutedEventArgs e)
        {
            Handel.Visibility = Visibility.Collapsed;
            Sasol.Visibility = Visibility.Collapsed;
            Verdi.Visibility = Visibility.Collapsed;
            VaalMall.Visibility = Visibility.Visible;
            Properties.Visibility = Visibility.Collapsed;

        }

        private void HomeVerdi_Click(object sender, RoutedEventArgs e)
        {
            Handel.Visibility = Visibility.Collapsed;
            Sasol.Visibility = Visibility.Collapsed;
            Verdi.Visibility = Visibility.Visible;
            VaalMall.Visibility = Visibility.Collapsed;
            Properties.Visibility = Visibility.Collapsed;

        }

        private void Reciepts_Click(object sender, RoutedEventArgs e)
        {
            // Your code for receipts folder
        }

        
        

        

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Visibility = Visibility.Visible;
            Handel.Visibility = Visibility.Collapsed;
            Sasol.Visibility = Visibility.Collapsed;
            Verdi.Visibility = Visibility.Collapsed;
            VaalMall.Visibility = Visibility.Collapsed;
        }

        private void HandelWater_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HandelElec_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VerdiWater_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VerdiElec_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SasolWater_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SasolElec_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VaalMallWater_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VaalMallElec_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
