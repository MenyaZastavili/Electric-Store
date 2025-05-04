using ElectricStore.ApplicationData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElectricStore.Tests
{
    [TestClass]
    public class ProductTests
    {
        // Создает тестовый продукт
        private Product CreateTestProduct()
        {
            return new Product
            {
                Id = 1,
                Name = "ТестовыйТелефон",
                Category = "Смартфоны",
                Price = 599.99m,
                StockQuantity = 10
            };
        }

        [TestMethod]
        public void Quantity_SetValid_SetsCorrectly()
        {
            var product = CreateTestProduct();
            product.Quantity = 5;
            Assert.AreEqual(5, product.Quantity, "Количество должно быть 5.");
        }

        [TestMethod]
        public void Quantity_SetExcessive_SetsToStock()
        {
            var product = CreateTestProduct();
            product.Quantity = 15;
            Assert.AreEqual(10, product.Quantity, "Количество должно быть равно StockQuantity (10).");
        }

        [TestMethod]
        public void Quantity_SetZero_SetsZero()
        {
            var product = CreateTestProduct();
            product.Quantity = 0;
            Assert.AreEqual(0, product.Quantity, "Количество должно быть 0.");
        }
    }
}