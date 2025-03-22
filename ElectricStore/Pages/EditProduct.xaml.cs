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

namespace ElectricStore.Pages.Order
{
    public partial class EditProduct : Page
    {
        public EditProduct(Products product)
        {
            tempProduct = product;
            InitializeComponent();
            this.DataContext = this;
        }

        public Products tempProduct { get; set; }

        private void editProduct(object sender, RoutedEventArgs e)
        {
            if (tempProduct.Name.Equals("") || tempProduct.Category.Equals("") || tempProduct.Price.Equals("") || tempProduct.StockQuantity.Equals(""))
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
                        this.NavigationService.Navigate(new Pages.PageAdmin());
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
