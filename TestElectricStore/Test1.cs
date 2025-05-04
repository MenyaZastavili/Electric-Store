using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ElectricStore;

namespace TestElectricStore
{
    [TestClass]
    public class RegistrationTests
    {
        [TestMethod]
        public void btnRegIn_Click_ShouldShowErrorMessage_WhenFieldsAreEmpty()
        {
            // Arrange
            var mockDb = new Mock<YourDbContext>();
            var mockClients = new Mock<DbSet<Clients>>();
            mockDb.Setup(db => db.Clients).Returns(mockClients.Object);

            var window = new YourWindow(); // Замените на ваше окно
            window.UName = new TextBox { Text = "" };
            window.UEmail = new TextBox { Text = "" };
            window.UPassword = new PasswordBox { Password = "" };

            var messageBoxShown = false;
            MessageBox.Show = (message, caption, button, icon) =>
            {
                messageBoxShown = true;
                Assert.AreEqual("Все поля должны быть заполнены!", message);
            };

            // Act
            window.btnRegIn_Click(null, null);

            // Assert
            Assert.IsTrue(messageBoxShown);
        }

        [TestMethod]
        public void btnRegIn_Click_ShouldShowErrorMessage_WhenEmailAlreadyExists()
        {
            // Arrange
            var mockDb = new Mock<YourDbContext>();
            var mockClients = new Mock<DbSet<Clients>>();
            mockClients.Setup(c => c.Count(It.IsAny<Func<Clients, bool>>())).Returns(1);
            mockDb.Setup(db => db.Clients).Returns(mockClients.Object);

            var window = new YourWindow(); // Замените на ваше окно
            window.UName = new TextBox { Text = "Test Name" };
            window.UEmail = new TextBox { Text = "test@example.com" };
            window.UPassword = new PasswordBox { Password = "password" };

            var messageBoxShown = false;
            MessageBox.Show = (message, caption, button, icon) =>
            {
                messageBoxShown = true;
                Assert.AreEqual("Пользователь с таким логином есть!", message);
            };

            // Act
            window.btnRegIn_Click(null, null);

            // Assert
            Assert.IsTrue(messageBoxShown);
        }

        [TestMethod]
        public void btnRegIn_Click_ShouldAddUser_WhenAllFieldsAreValid()
        {
            // Arrange
            var mockDb = new Mock<YourDbContext>();
            var mockClients = new Mock<DbSet<Clients>>();
            mockClients.Setup(c => c.Count(It.IsAny<Func<Clients, bool>>())).Returns(0);
            mockDb.Setup(db => db.Clients).Returns(mockClients.Object);

            var window = new YourWindow(); // Замените на ваше окно
            window.UName = new TextBox { Text = "Test Name" };
            window.UEmail = new TextBox { Text = "test@example.com" };
            window.UPassword = new PasswordBox { Password = "password" };

            var messageBoxShown = false;
            MessageBox.Show = (message, caption, button, icon) =>
            {
                messageBoxShown = true;
                Assert.AreEqual("Данные успешно добавлены!", message);
            };

            // Act
            window.btnRegIn_Click(null, null);

            // Assert
            Assert.IsTrue(messageBoxShown);
            mockClients.Verify(c => c.Add(It.IsAny<Clients>()), Times.Once);
            mockDb.Verify(db => db.SaveChanges(), Times.Once);
        }
    }
}
