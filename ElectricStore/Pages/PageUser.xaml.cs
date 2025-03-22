using ElectricStore.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectricStore.Pages
{
    public partial class PageUser : Page
    {

        public PageUser()
        {
            InitializeComponent();
            LoadProducts();
            LoadHistoryOrder();
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

        int clientId = (int)Application.Current.Properties["CurrentClientId"];

        private void LoadHistoryOrder()
        {
            try
            {
                OrderUserList.ItemsSource = AppConnect.model0db.Orders
                .Where(o => o.ClientId == clientId)
                .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }

        private void SearchProduct(object sender, TextChangedEventArgs e)
        {
            try
            {
                string productName = SearchInput.Text;
                var dataFilter = AppConnect.model0db.Products.Where(p => p.Name.Contains(productName));
                ProductList.ItemsSource = dataFilter.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске товаров: {ex.Message}");
            }
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
                LoadHistoryOrder();

                UpdateTotalAmount();
                MessageBox.Show("Продукт добавлен в корзину!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
