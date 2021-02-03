using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoWatcher.Domain.Builders;


namespace CryptoWatcher.Tests.Domain.Builders
{
    [TestClass]
    public class BuildHypes
    {
        [TestMethod]
        public void Test_1()
        {
            // Arrange
            var values = new decimal[] {2, -10, -10, -10, -15};

            // Act
            IndicatorBuilder.BuildHypes(values);

            // Assert
            Assert.AreEqual(true, values[0] >= 0);
            Assert.AreEqual(true, values[1] == 0);
            Assert.AreEqual(true, values[2] == 0);
            Assert.AreEqual(true, values[3] == 0);
            Assert.AreEqual(true, values[4] == 0);
        }

        [TestMethod]
        public void Test_2()
        {
            // Arrange
            var values = new decimal[] { 5, 1, 1, 1, -5 };

            // Act
            IndicatorBuilder.BuildHypes(values);

            // Assert
            Assert.AreEqual(true, values[0] >= 0);
            Assert.AreEqual(true, values[1] >= 0);
            Assert.AreEqual(true, values[2] >= 0);
            Assert.AreEqual(true, values[3] >= 0);
            Assert.AreEqual(true, values[4] == 0);
        }

        [TestMethod]
        public void Test_3()
        {
            // Arrange
            var values = new decimal[] { 6, 1, 1, 1, 1 };

            // Act
            IndicatorBuilder.BuildHypes(values);

            // Assert
            Assert.AreEqual(true, values[0] >= 0);
            Assert.AreEqual(true, values[1] == 0);
            Assert.AreEqual(true, values[2] == 0);
            Assert.AreEqual(true, values[3] == 0);
            Assert.AreEqual(true, values[4] == 0);
        }

        [TestMethod]
        public void Test_4()
        {
            // Arrange
            var values = new decimal[] { 1, -6, -6, -6, -6 };

            // Act
           IndicatorBuilder.BuildHypes(values);

            // Assert
            Assert.AreEqual(true, values[0] >= 0);
            Assert.AreEqual(true, values[1] == 0);
            Assert.AreEqual(true, values[2] == 0);
            Assert.AreEqual(true, values[3] == 0);
            Assert.AreEqual(true, values[4] == 0);
        }

        [TestMethod]
        public void Test_5()
        {
            // Arrange
            var values = new decimal[] { 100, 0, 0, 0, 0 };

            // Act
           IndicatorBuilder.BuildHypes(values);

            // Assert
            Assert.AreEqual(true, values[0] >= 0);
            Assert.AreEqual(true, values[1] == 0);
            Assert.AreEqual(true, values[2] == 0);
            Assert.AreEqual(true, values[3] == 0);
            Assert.AreEqual(true, values[4] == 0);
        }

        [TestMethod]
        public void Test_6()
        {
            // Arrange
            var values = new decimal[] { 50, 0, 0, 0, -50 };

            // Act
           IndicatorBuilder.BuildHypes(values);

            // Assert
            Assert.AreEqual(true, values[0] >= 0);
            Assert.AreEqual(true, values[1] == 0);
            Assert.AreEqual(true, values[2] == 0);
            Assert.AreEqual(true, values[3] == 0);
            Assert.AreEqual(true, values[4] == 0);
        }
    }
}
