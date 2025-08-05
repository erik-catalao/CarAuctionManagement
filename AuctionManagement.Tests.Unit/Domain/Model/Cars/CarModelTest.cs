using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model;
using AuctionManagement.Domain.Model.Cars;

namespace AuctionManagement.Tests.Unit.Domain.Model.Cars
{
    [TestClass]
    [TestCategory("Unit")]
    public class CarModelTest
    {
        #region Hatchback-related tests.
        [TestMethod]
        public void CreateValidHatchback()
        {
            var result = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 2);

            Assert.AreEqual("SET3DKJJFW9KY1285", result.VIN);
            Assert.AreEqual("Honda", result.Manufacturer);
            Assert.AreEqual("Accord", result.Model);
            Assert.AreEqual(2012, result.Year);
            Assert.AreEqual(10.00m, result.StartingBid);
            Assert.AreEqual(CarType.HATCHBACK, result.CarType);
            Assert.AreEqual(2, result.NumberOfDoors);
        }

        [TestMethod]
        public void CreateInvalidHatchback_ThrowsException()
        {
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 1));
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, -4));
        }
        #endregion

        #region Sudan-related tests.
        [TestMethod]
        public void CreateValidSudan()
        {
            var result = new Sudan("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 2);

            Assert.AreEqual("SET3DKJJFW9KY1285", result.VIN);
            Assert.AreEqual("Honda", result.Manufacturer);
            Assert.AreEqual("Accord", result.Model);
            Assert.AreEqual(2012, result.Year);
            Assert.AreEqual(10.00m, result.StartingBid);
            Assert.AreEqual(CarType.SUDAN, result.CarType);
            Assert.AreEqual(2, result.NumberOfDoors);
        }

        [TestMethod]
        public void CreateInvalidSudan_ThrowsException()
        {
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Sudan("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 1));
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Sudan("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, -4));
        }
        #endregion

        #region SUV-related tests.
        [TestMethod]
        public void CreateValidSUV()
        {
            var result = new SUV("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 1);

            Assert.AreEqual("SET3DKJJFW9KY1285", result.VIN);
            Assert.AreEqual("Honda", result.Manufacturer);
            Assert.AreEqual("Accord", result.Model);
            Assert.AreEqual(2012, result.Year);
            Assert.AreEqual(10.00m, result.StartingBid);
            Assert.AreEqual(CarType.SUV, result.CarType);
            Assert.AreEqual(1, result.NumberOfSeats);
        }

        [TestMethod]
        public void CreateInvalidSUV_ThrowsException()
        {
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new SUV("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 0));
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new SUV("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, -1));
        }
        #endregion

        #region Truck-related tests.
        [TestMethod]
        public void CreateValidTruck()
        {
            var result = new Truck("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 500);

            Assert.AreEqual("SET3DKJJFW9KY1285", result.VIN);
            Assert.AreEqual("Honda", result.Manufacturer);
            Assert.AreEqual("Accord", result.Model);
            Assert.AreEqual(2012, result.Year);
            Assert.AreEqual(10.00m, result.StartingBid);
            Assert.AreEqual(CarType.TRUCK, result.CarType);
            Assert.AreEqual(500, result.LoadCapacity);
        }

        [TestMethod]
        public void CreateInvalidTruck_ThrowsException()
        {
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 0));
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, -15000));
        }
        #endregion

        #region Base Car validation tests, using Truck as the implementation.

        [TestMethod]
        public void CreateInvalidCar_ThrowsException()
        {
            // Incorrect VIN
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("NotAVin", "Honda", "Accord", 2012, 10.00m, 500));

            // No manufacturer
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("SET3DKJJFW9KY1285", "", "Accord", 2012, 10.00m, 500));

            // No model
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("SET3DKJJFW9KY1285", "Honda", "", 2012, 10.00m, 500));

            // Invalid year.
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("SET3DKJJFW9KY1285", "Honda", "Accord", 2026, 10.00m, 500));

            // Invalid starting bid.
            Assert.ThrowsExactly<InvalidCarParametersException>(() => new Truck("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 0, 500));
        }

        #endregion
    }
}
