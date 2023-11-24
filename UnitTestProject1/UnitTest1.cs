using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            [TestMethod]
            public void TestTotalGreaterThanThreshold()
            {
                // Arrange
                List<decimal> donations = new List<decimal> { 50, 60, 70 };
                decimal threshold = 150;

                // Act
                bool result = TotalGreaterThanThreshold(donations, threshold);

                // Assert
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void TestGoodsSufficient()
            {
                // Arrange
                Dictionary<string, decimal> goodsAvailable = new Dictionary<string, decimal>
            {
                { "Item1", 50 },
                { "Item2", 60 },
                { "Item3", 70 }
            };

                decimal threshold = 100;

                // Act
                bool result = GoodsSufficient(goodsAvailable, threshold);

                // Assert
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void TestIfAllocationMoney()
            {
                // Arrange
                Dictionary<string, decimal> allocatedMoney = new Dictionary<string, decimal>
            {
                { "Category1", 30 },
                { "Category2", 40 },
                { "Category3", 20 }
            };

                decimal availableMoney = 100;

                // Act
                bool result = IsAllocationMoneySufficient(allocatedMoney, availableMoney);

                // Assert
                Assert.IsTrue(result);
            }

            public bool TotalGreaterThanThreshold(List<decimal> donations, decimal threshold)
            {
                decimal totalDonation = donations.Sum();
                return totalDonation > threshold;
            }

            public bool GoodsSufficient(Dictionary<string, decimal> goodsAvailable, decimal threshold)
            {
                decimal totalGoods = goodsAvailable.Values.Sum();
                return totalGoods > threshold;
            }

            public bool IsAllocationMoneySufficient(Dictionary<string, decimal> allocatedMoney, decimal availableMoney)
            {
                decimal totalAllocatedMoney = allocatedMoney.Values.Sum();
                return totalAllocatedMoney <= availableMoney;
            }
        }
    }
}
