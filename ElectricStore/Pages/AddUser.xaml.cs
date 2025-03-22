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
using ElectricStore;
using ElectricStore.ApplicationData;
using ElectricStore.Pages;

namespace ElectricStore.Pages
{
    public partial class AddUser : Page
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userName = UserName.Text;
                var userEmail = Email.Text;
                var userPhone = PhoneNumber.Text;
                int userRole = int.Parse(Role.Text);

                var clients = new Clients
                {
                    FullName = userName,
                    Email = userEmail,
                    PhoneNumber = userPhone,
                    Password = "password",
                    Role = userRole
                };

                AppConnect.model0db.Clients.Add(clients);
                AppConnect.model0db.SaveChanges();

                if (MessageBox.Show("Запись успешно добавлена", "Success", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    this.NavigationService.Navigate(new PageAdmin());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении данных!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
