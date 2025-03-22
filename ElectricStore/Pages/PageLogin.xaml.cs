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
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void In_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.model0db.Clients.FirstOrDefault(x => x.Email == UserEmail.Text && x.Password == UserPassword.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Application.Current.Properties["CurrentClientId"] = userObj.Id;
                    Application.Current.Properties["CurrentClientRole"] = userObj.Role;

                    switch (userObj.Role)
                    {
                        case 1: 
                            MessageBox.Show("Здравствуйте, Администратор " + userObj.FullName + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.NavigationService.Navigate(new Pages.PageAdmin());
                            break;
                        case 2:
                            MessageBox.Show("Здравствуйте, Пользователь " + userObj.FullName + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.NavigationService.Navigate(new Pages.PageUser());
                            break;
                        default:
                            MessageBox.Show("Данные не обнаружены ", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ошибка " + ex.Message.ToString() + "Критическая работа приложения!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.PageCreateAcc());
        }
    }
}
