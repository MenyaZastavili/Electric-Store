using ElectricStore.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace ElectricStore
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.model0db = new ElectricStoreEntities();
            Fr.Navigate(new Pages.PageLogin());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Fr.CanGoBack)
            {
                Fr.GoBack();
            }
            else
            {
                MessageBox.Show("Вы находитесь на главной странице", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
