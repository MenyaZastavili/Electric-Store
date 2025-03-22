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
    public partial class EditUser : Page
    {
        public EditUser(Clients user)
        {
            tempUser = user;
            InitializeComponent();
            this.DataContext = this;
        }

        public Clients tempUser { get; set; }

        private void editUser(object sender, RoutedEventArgs e)
        {
            if (tempUser.FullName.Equals("") || tempUser.Email.Equals("") || tempUser.Password.Equals("") || tempUser.PhoneNumber.Equals("") || tempUser.Role.Equals(""))
            {
                MessageBox.Show("Произошла ошибка, при редактировании! Введены некоректные данные!", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    AppConnect.model0db.SaveChanges();
                    if (MessageBox.Show("Запись успешно обновлена", "Success", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        this.NavigationService.Navigate(new PageAdmin());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка, при редактировании!", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
