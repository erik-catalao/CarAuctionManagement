using AuctionManagement.Domain.Model.Cars;
using AuctionManagement.Infrastructure.Cars;

namespace AuctionManagement.Tests.Unit.Infrastructure.Cars
{
    [TestClass]
    [TestCategory("Unit")]
    public sealed class CarRepositoryTest
    {

        private CarRepository victim = new CarRepository();

        [TestInitialize]
        public void Startup()
        {
            victim.AddCar(new Hatchback("SET3DKJJFW9KY1281", "Honda", "Accord", 2012, 10.00m, 2));
            victim.AddCar(new Truck("SET3DKJJFW9KY1282", "Honda", "Accord", 2012, 10.00m, 1500));
            victim.AddCar(new Sudan("SET3DKJJFW9KY1283", "Honda", "Accord", 2012, 10.00m, 4));
            victim.AddCar(new SUV("SET3DKJJFW9KY1284", "Honda", "Accord", 2012, 10.00m, 1));
        }

        #region GetAllCars
        [TestMethod]
        public async Task GetAllCars()
        {
            var result = await victim.GetAllCars();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public async Task GetAllCars_ValidateEncapsulation()
        {
            var firstAllCars = await victim.GetAllCars();
            var hatchback = firstAllCars.First();
            hatchback.MarkAsUnavailable();

            var secondAllCars = await victim.GetAllCars();
            var result = secondAllCars.First();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAvailable);
        }
        #endregion

        #region AddCar
        [TestMethod]
        public async Task AddCar_Success()
        {
            var result = await victim.AddCar(new Hatchback("SET3DKJJFW9KY1289", "Honda", "Accord", 2012, 10.00m, 2));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AddCar_Failure()
        {
            var result = await victim.AddCar(new Hatchback("SET3DKJJFW9KY1282", "Honda", "Accord", 2012, 10.00m, 2));

            Assert.IsFalse(result);
        }
        #endregion

        #region GetCarByVIN
        [TestMethod]
        public async Task GetCarByVIN_Success()
        {
            var result = await victim.GetCarByVIN("SET3DKJJFW9KY1282");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetCarByVIN_Failure()
        {
            var result = await victim.GetCarByVIN("SET3DKJJFW9KY1299");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCarByVIN_Encapsulation()
        {
            var firstSearch = await victim.GetCarByVIN("SET3DKJJFW9KY1282");
            firstSearch.MarkAsUnavailable();

            var result = await victim.GetCarByVIN("SET3DKJJFW9KY1282");

            Assert.IsTrue(result.IsAvailable);
        }
        #endregion

        #region MarkCarAsUnavailable
        [TestMethod]
        public async Task MarkCarAsUnavailable_Success()
        {
            var result = await victim.MarkCarAsUnavailable("SET3DKJJFW9KY1281");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task MarkCarAsUnavailable_Failure()
        {
            var result = await victim.MarkCarAsUnavailable("SET3DKJJFW9KY1111");
            Assert.IsFalse(result);
        }
        #endregion
    }
}
