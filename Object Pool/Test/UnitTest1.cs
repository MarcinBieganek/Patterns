using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zadanie_3
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestInvalidCapacity()
        {
            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    Airport airport = new Airport(-1);
                });
        }

        [TestMethod]
        public void TestValidCapacity()
        {
            Airport airport = new Airport(1);
            Plain plain = airport.AcquireReusable();

            Assert.IsNotNull(plain);
        }

        [TestMethod]
        public void CapacityDepleted()
        {
            Airport airport = new Airport(1);
            Plain plain = airport.AcquireReusable();

            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    Plain plain = airport.AcquireReusable();
                });
        }

        [TestMethod]
        public void ReusedObject()
        {
            Airport airport = new Airport(1);
            Plain plain = airport.AcquireReusable();

            airport.ReleaseReusable(plain);

            Plain plain2 = airport.AcquireReusable();

            Assert.AreEqual(plain, plain2);
        }

        
        [TestMethod]
        public void ReleaseInvalidReusable()
        {
            Airport airport = new Airport(1);
            Airport airport2 = new Airport(1);

            Plain plain = airport2.AcquireReusable();

            Assert.ThrowsException<ArgumentException>(
                () =>
                {
                    airport.ReleaseReusable(plain);
                });
        }
    }
}
