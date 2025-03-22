using ElectricStore.ApplicationData;
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

namespace ElectricStore.Pages
{
    public partial class PageCreateAcc : Page
    {
        public PageCreateAcc()
        {
            InitializeComponent();
        }

        private void btnRegIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UName.Text) || string.IsNullOrEmpty(UEmail.Text) || string.IsNullOrEmpty(UPassword.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AppConnect.model0db.Clients.Count (x => x.Email == UEmail.Text)>0)
            {
                MessageBox.Show("Пользователь с таким логином есть!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                Clients userObj = new Clients()
                {
                    FullName = UName.Text,
                    Email = UEmail.Text,
                    Password = UPassword.Password,
                    PhoneNumber = "11111111111",
                    Role = 2
                };

                AppConnect.model0db.Clients.Add(userObj);
                AppConnect.model0db.SaveChanges();
                MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении данных!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UPassword.Password != txbPass.Password)
            {
                btncreate.IsEnabled = false;
                UPassword.Background = Brushes.LightCoral;
                UPassword.BorderBrush = Brushes.Red;
            }
            else
            {
                btncreate.IsEnabled = true;
                UPassword.Background = Brushes.LightGreen;
                UPassword.BorderBrush = Brushes.Green;
            }
        }
    }
}
