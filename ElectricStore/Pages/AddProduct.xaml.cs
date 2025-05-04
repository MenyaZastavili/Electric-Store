using ElectricStore.ApplicationData;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class AddProduct : Page
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productName = ProductName.Text;
                var productCategory = ProductCategory.Text;
                int productPrice = int.Parse(ProductPrice.Text);
                int productStockQuantity = int.Parse(ProductStockQuantity.Text);

                var existingProduct = AppConnect.model0db.Products
                    .FirstOrDefault(p => p.Name == productName && p.Category == productCategory);

                if (existingProduct != null)
                {

                    existingProduct.StockQuantity += productStockQuantity;

                    AppConnect.model0db.SaveChanges();

                    MessageBox.Show("Количество товара обновлено", "Обновлено", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var product = new Products
                    {
                        Name = productName,
                        Category = productCategory,
                        Price = productPrice,
                        StockQuantity = productStockQuantity
                    };

                    AppConnect.model0db.Products.Add(product);
                    AppConnect.model0db.SaveChanges();

                    MessageBox.Show("Новый продукт добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.NavigationService.Navigate(new PageAdmin());
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при добавлении данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
