using ElectricStore.Tests;
using System.Xml.Linq;

namespace Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void RegisterTest()
        {
            string Expected = "Все поля должны быть заполнены!";

            TestFunctions test = new TestFunctions();

            Assert.Equals(Expected, test.BtnRegIn_Click(" ", " ", " ", " "));

        }
    }
}
