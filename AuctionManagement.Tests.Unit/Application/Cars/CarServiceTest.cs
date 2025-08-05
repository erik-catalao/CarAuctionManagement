using AuctionManagement.Application.Cars;
using AuctionManagement.Application.Models.DTOs.Cars;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model;
using AuctionManagement.Domain.Model.Cars;
using AuctionManagement.Domain.Repositories;
using Moq;

namespace AuctionManagement.Tests.Unit.Application.Cars
{
    [TestClass]
    [TestCategory("Unit")]
    public sealed class CarServiceTest
    {
        private Mock<ICarRepository> mockRepo = new Mock<ICarRepository>();

        private ICarService victim;

        [TestInitialize]
        public void Startup()
        {
            victim = new CarService(mockRepo.Object);
        }

        [TestMethod]
        public async Task CreateCar_Success()
        {
            var carDto = new TruckDTO
            {
                VIN = "SET3DKJJFW9KY1285",
                Manufacturer = "Honda",
                Model = "Accord",
                Year = 2012,
                StartingBid = 25000.00m,
                CarType = CarType.TRUCK,
                LoadCapacity = 150,
                IsAvailable = true
            };
            var request = new CreateCarRequest { Car = carDto };
            var expectedResponse = new CreateCarResponse { CarDTO = carDto };
            mockRepo.Setup(repo => repo.AddCar(It.IsAny<Car>()))
                .ReturnsAsync(true);

            var result = await victim.CreateCar(request);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task CreateCar_FailureToAddOnRepo()
        {
            var carDto = new TruckDTO
            {
                VIN = "SET3DKJJFW9KY1285",
                Manufacturer = "Honda",
                Model = "Accord",
                Year = 2012,
                StartingBid = 25000.00m,
                CarType = CarType.TRUCK,
                LoadCapacity = 150
            };
            var request = new CreateCarRequest { Car = carDto };
            mockRepo.Setup(repo => repo.AddCar(It.IsAny<Car>()))
                .ReturnsAsync(false);

            await Assert.ThrowsExactlyAsync<DuplicateCarException>(async () => await victim.CreateCar(request));
        }

        [TestMethod]
        public async Task CreateCar_FailureToMapCarToDomain()
        {
            var carDto = new CarDTO
            {
                VIN = "SET3DKJJFW9KY1285",
                Manufacturer = "Honda",
                Model = "Accord",
                Year = 2012,
                StartingBid = 25000.00m,
                CarType = CarType.TRUCK
            };

            var request = new CreateCarRequest { Car = carDto };

            await Assert.ThrowsExactlyAsync<InvalidCarParametersException>(async () => await victim.CreateCar(request));
        }

        [TestMethod]
        [DataRow(CarType.HATCHBACK, null, null, null)]
        [DataRow(null, "Accord", null, null)]
        [DataRow(null, null, "Honda", null)]
        [DataRow(null, null, null, 2012)]
        [DataRow(CarType.HATCHBACK, "Accord", "Honda", 2012)]
        public async Task QueryCar(CarType? type, string? model, string? manufacturer, int? year)
        {
            var returnedCars = new List<Car>
            {
                new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 2)
            };
            var expectedCarDtoResult = new HatchbackDTO
            {
                VIN = "SET3DKJJFW9KY1285",
                Manufacturer = "Honda",
                Model = "Accord",
                Year = 2012,
                StartingBid = 10.00m,
                CarType = CarType.HATCHBACK,
                NumberOfDoors = 2,
                IsAvailable = true
            };

            mockRepo.Setup(repo => repo.GetAllCars())
                .ReturnsAsync(returnedCars);

            var result = await victim.QueryCars(type, model, manufacturer, year);

            Assert.IsNotEmpty(result.CarDTOs);
            Assert.IsTrue(result.CarDTOs.Contains(expectedCarDtoResult));
        }
    }
}
