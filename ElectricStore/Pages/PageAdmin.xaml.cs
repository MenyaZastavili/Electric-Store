using ElectricStore.ApplicationData;
using ElectricStore.Pages.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectricStore.Pages
{

    public partial class PageAdmin : Page
    {

        public PageAdmin()
        {
            InitializeComponent();
            LoadProducts();
            LoadClients();
            LoadOrder();
            LoadOrderDetail();
            UpdateTotalAmount();
        }

        private void LoadProducts()
        {
            try
            {
                var products = AppConnect.model0db.Products.ToList();
                ProductList.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}");
            }
        }

        private void LoadClients()
        {
            try
            {
                var clients = AppConnect.model0db.Clients.ToList();
                UserList.ItemsSource = clients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
            }
        }

        private void LoadOrder()
        {
            try
            {
                var order = AppConnect.model0db.Orders.ToList();
                OrderList.ItemsSource = order;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }

        private void LoadOrderDetail()
        {
            try
            {
                var orderD = AppConnect.model0db.OrderDetails.ToList();
                OrderDList.ItemsSource = orderD;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }

        private void SearchProduct(object sender, TextChangedEventArgs e)
        {
            string productName = SearchInputProduct.Text;
            var dataFilter = AppConnect.model0db.Products.Where(u => u.Name.Contains(productName));
            ProductList.ItemsSource = dataFilter.ToList();
        }

        private void SearchUser(object sender, TextChangedEventArgs e)
        {
            string userName = SearchInputUser.Text;
            var dataFilter = AppConnect.model0db.Clients.Where(u => u.FullName.Contains(userName));
            UserList.ItemsSource = dataFilter.ToList();
        }

        private void SearchOrder(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(SearchInputOrder.Text, out int clientId))
            {
                var dataFilter = AppConnect.model0db.Orders.Where(u => u.ClientId == clientId);
                OrderList.ItemsSource = dataFilter.ToList();
            }
            else
            {
                OrderList.ItemsSource = AppConnect.model0db.Orders.ToList();
            }
        }

        private void SearchDOrder(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(SearchInputOrderD.Text, out int orderId))
            {
                var dataFilter = AppConnect.model0db.OrderDetails.Where(u => u.OrderId == orderId);
                OrderDList.ItemsSource = dataFilter.ToList();
            }
            else
            {
                OrderDList.ItemsSource = AppConnect.model0db.OrderDetails.ToList();
            }
        }

        private void deleteProduct(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var product = ProductList.SelectedItem as Products;

                AppConnect.model0db.Products.Remove(product);
                AppConnect.model0db.SaveChanges();

                MessageBox.Show("Запись успешно удалена!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProducts();
            }
        }

        private void editProduct(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditProduct(ProductList.SelectedItem as Products));
        }

        private void deleteUser(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var client = UserList.SelectedItem as Clients;

                AppConnect.model0db.Clients.Remove(client);
                AppConnect.model0db.SaveChanges();

                MessageBox.Show("Запись успешно удалена!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProducts();
            }
        }

        private void editUser(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditUser(UserList.SelectedItem as Clients));
        }

        private void UpdateTotalAmount()
        {
            try
            {
                if (!Application.Current.Properties.Contains("CurrentClientId") || Application.Current.Properties["CurrentClientId"] == null)
                {
                    TotalAmountTextBlock.Text = "Общая сумма: 0";
                    return;
                }

                int clientId = (int)Application.Current.Properties["CurrentClientId"];

                var order = AppConnect.model0db.Orders.FirstOrDefault(o => o.ClientId == clientId && o.Status == 1);

                if (order != null)
                {
                    var totalAmount = AppConnect.model0db.OrderDetails
                        .Where(od => od.OrderId == order.Id)
                        .Sum(od => od.Quantity * od.PricePerUnit);

                    TotalAmountTextBlock.Text = $"Общая сумма: {totalAmount} руб.";
                }
                else
                {
                    TotalAmountTextBlock.Text = "Общая сумма: 0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении суммы заказа: {ex.Message}");
            }
        }

        private void orderProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button == null || button.Tag == null)
                {
                    MessageBox.Show("Ошибка: ID продукта отсутствует.");
                    return;
                }

                if (!int.TryParse(button.Tag.ToString(), out int productId))
                {
                    MessageBox.Show("Ошибка при получении ID продукта.");
                    return;
                }

                if (!Application.Current.Properties.Contains("CurrentClientId") || Application.Current.Properties["CurrentClientId"] == null)
                {
                    MessageBox.Show("Ошибка: CurrentClientId не установлен.");
                    return;
                }

                int clientId = (int)Application.Current.Properties["CurrentClientId"];

                var order = AppConnect.model0db.Orders.FirstOrDefault(o => o.ClientId == clientId && o.Status == 1);

                if (order == null)
                {
                    order = new Orders
                    {
                        ClientId = clientId,
                        OrderDate = DateTime.Now,
                        Status = 1,
                        TotalAmount = 0
                    };
                    AppConnect.model0db.Orders.Add(order);
                    AppConnect.model0db.SaveChanges();
                }

                var orderDetail = AppConnect.model0db.OrderDetails.FirstOrDefault(od => od.OrderId == order.Id && od.ProductId == productId);

                if (orderDetail == null)
                {
                    var product = AppConnect.model0db.Products.Find(productId);
                    if (product != null)
                    {
                        if (product.StockQuantity > 0)
                        {
                            orderDetail = new OrderDetails
                            {
                                OrderId = order.Id,
                                ProductId = productId,
                                Quantity = 1,
                                PricePerUnit = product.Price
                            };
                            AppConnect.model0db.OrderDetails.Add(orderDetail);

                            product.StockQuantity--;
                            AppConnect.model0db.SaveChanges();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Продукт не найден.");
                        return;
                    }
                }
                else
                {
                    orderDetail.Quantity++;

                    var product = AppConnect.model0db.Products.Find(productId);
                    if (product != null && product.StockQuantity > 0)
                    {
                        product.StockQuantity--;
                    }
                }

                order.TotalAmount = AppConnect.model0db.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .Sum(od => od.Quantity * od.PricePerUnit);

                AppConnect.model0db.SaveChanges();

                LoadProducts();

                UpdateTotalAmount();
                MessageBox.Show("Продукт добавлен в корзину!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void readyProduct(object sender, RoutedEventArgs e)
        {
            try
            {

                var button = sender as Button;
                if (button == null || OrderList.SelectedItem == null)
                {
                    MessageBox.Show("Ошибка: Заказ не выбран.");
                    return;
                }

                var order = OrderList.SelectedItem as Orders;
                if (order == null)
                {
                    MessageBox.Show("Ошибка: Заказ не найден.");
                    return;
                }

                order.Status = 2;

                AppConnect.model0db.SaveChanges();

                LoadOrder();

                MessageBox.Show("Статус заказа изменён на 'Готов'", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при изменении статуса заказа: {ex.Message}");
            }
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.AddUser());
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.AddProduct());
        }
    }
}
