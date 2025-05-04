using ElectricStore.ApplicationData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectricStore.Tests
{
    [TestClass]
    public class OrderTests
    {
        // Создает тестовый заказ
        private Order CreateTestOrder()
        {
            return new Order
            {
                Id = 1,
                ClientId = 100,
                OrderDate = DateTime.Today,
                TotalAmount = 0,
                OrderDetails = new List<OrderDetail>()
            };
        }

        // Создает тестовую деталь заказа
        private OrderDetail CreateTestOrderDetail(int productId, int quantity, decimal pricePerUnit)
        {
            return new OrderDetail
            {
                Id = productId,
                ProductId = productId,
                Quantity = quantity,
                PricePerUnit = pricePerUnit
            };
        }

        [TestMethod]
        public void AddOrderDetail_ShouldIncreaseOrderDetailsCount()
        {
            var order = CreateTestOrder();
            var detail = CreateTestOrderDetail(1, 2, 499.99m);

            order.OrderDetails.Add(detail);

            Assert.AreEqual(1, order.OrderDetails.Count, "Деталь заказа должна быть добавлена.");
        }

        [TestMethod]
        public void CalculateTotalAmount_ShouldBeCorrect()
        {
            var order = CreateTestOrder();

            order.OrderDetails.Add(CreateTestOrderDetail(1, 2, 100m));
            order.OrderDetails.Add(CreateTestOrderDetail(2, 1, 300m));

            decimal expectedTotal = order.OrderDetails.Sum(d => d.Quantity * d.PricePerUnit);
            order.TotalAmount = expectedTotal;

            Assert.AreEqual(500m, order.TotalAmount, "Итоговая сумма заказа должна быть 500.");
        }
    }
}
