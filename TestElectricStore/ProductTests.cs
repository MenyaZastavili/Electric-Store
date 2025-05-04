using ElectricStore.ApplicationData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ElectricStore.Tests

{
    [TestClass]
    public class ProductTests
    {
        // Создает тестовый продукт с заранее заданными значениями
        private Product CreateTestProduct()
        {
            return new Product
            {
                Id = 1,
                Name = "ТестовыйТелефон",
                Category = "Смартфоны",
                Price = 599.99m,
                StockQuantity = 10,
                IsSelected = false
            };
        }

        [TestMethod]
        public void Quantity_SetValidQuantity_ReturnsCorrectQuantity()
        {
            // Подготовка
            var product = CreateTestProduct();
            int expectedQuantity = 5;

            // Действие
            product.Quantity = expectedQuantity;

            // Проверка
            Assert.AreEqual(expectedQuantity, product.Quantity, "Количество должно быть установлено в указанное значение.");
        }

        [TestMethod]
        public void Quantity_SetQuantityGreaterThanStock_SetsToStockQuantity()
        {
            // Подготовка
            var product = CreateTestProduct();
            int stockQuantity = product.StockQuantity;
            int excessiveQuantity = stockQuantity + 5;

            // Действие
            product.Quantity = excessiveQuantity;

            // Проверка
            Assert.AreEqual(stockQuantity, product.Quantity, "Количество должно быть ограничено значением StockQuantity.");
        }

        [TestMethod]
        public void Quantity_SetZeroQuantity_ReturnsZero()
        {
            // Подготовка
            var product = CreateTestProduct();
            int expectedQuantity = 0;

            // Действие
            product.Quantity = expectedQuantity;

            // Проверка
            Assert.AreEqual(expectedQuantity, product.Quantity, "Количество должно быть установлено в ноль.");
        }

        [TestMethod]
        public void Quantity_SetNegativeQuantity_SetsToZeroOrValid()
        {
            // Подготовка
            var product = CreateTestProduct();
            int negativeQuantity = -5;

            // Действие
            product.Quantity = negativeQuantity;

            // Проверка
            Assert.IsTrue(product.Quantity >= 0, "Количество не должно быть отрицательным.");
        }

        [TestMethod]
        public void Quantity_SetQuantityEqualToStock_ReturnsStockQuantity()
        {
            // Подготовка
            var product = CreateTestProduct();
            int expectedQuantity = product.StockQuantity;

            // Действие
            product.Quantity = expectedQuantity;

            // Проверка
            Assert.AreEqual(expectedQuantity, product.Quantity, "Количество должно быть равно StockQuantity.");
        }
    }
}