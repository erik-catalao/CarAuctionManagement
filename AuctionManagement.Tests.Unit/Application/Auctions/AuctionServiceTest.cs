using AuctionManagement.Application.Auctions;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model.Auctions;
using AuctionManagement.Domain.Model.Cars;
using AuctionManagement.Domain.Repositories;
using Moq;

namespace AuctionManagement.Tests.Unit.Application.Auctions
{
    [TestClass]
    [TestCategory("Unit")]
    public sealed class AuctionServiceTest
    {
        private Mock<IAuctionRepository> mockAuctionRepo = new Mock<IAuctionRepository>();
        private Mock<ICarRepository> mockCarRepo = new Mock<ICarRepository>();

        private IAuctionService victim;
        private readonly Car car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 2);

        [TestInitialize]
        public void Startup()
        {
            victim = new AuctionService(mockAuctionRepo.Object, mockCarRepo.Object);
        }

        #region StartAuction tests.
        [TestMethod]
        public async Task StartAuction_Success()
        {
            //var createdAuction = 
            var request = new StartAuctionRequest { VIN = "SET3DKJJFW9KY1285" };

            mockCarRepo.Setup(repo => repo.GetCarByVIN(It.IsAny<string>()))
                .ReturnsAsync(this.car);

            mockAuctionRepo.Setup(repo => repo.StartAuction(It.IsAny<Auction>()))
                .ReturnsAsync(true);

            var result = await victim.StartAuction(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(car.VIN, result.AuctionDTO.CarVIN);
            Assert.AreEqual(car.StartingBid, result.AuctionDTO.StartingBid);
            Assert.AreEqual(car.StartingBid, result.AuctionDTO.LatestBid);
            Assert.IsNotNull(result.AuctionDTO.AuctionStartTime);
            Assert.IsNotNull(result.AuctionDTO.Id);
            Assert.IsFalse(result.AuctionDTO.IsCompleted);
        }

        [TestMethod]
        public async Task StartAuction_ThrowsCarNotFoundException()
        {
            //var createdAuction = 
            var request = new StartAuctionRequest { VIN = "SET3DKJJFW9KY1285" };

            Car nullCarResult = null;

            mockCarRepo.Setup(repo => repo.GetCarByVIN(It.IsAny<string>()))
                .ReturnsAsync(nullCarResult);

            await Assert.ThrowsExactlyAsync<CarNotFoundException>(async () => await victim.StartAuction(request));
        }

        [TestMethod]
        public async Task StartAuction_ThrowsCarUnavailableException()
        {
            //var createdAuction = 
            var request = new StartAuctionRequest { VIN = "SET3DKJJFW9KY1285" };

            mockCarRepo.Setup(repo => repo.GetCarByVIN(It.IsAny<string>()))
                .ReturnsAsync(new Truck("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10.00m, 2, false));

            await Assert.ThrowsExactlyAsync<CarNotAvailableException>(async () => await victim.StartAuction(request));
        }

        [TestMethod]
        public async Task StartAuction_ThrowsCarAlreadyInActiveAuctionException()
        {
            //var createdAuction = 
            var request = new StartAuctionRequest { VIN = "SET3DKJJFW9KY1285" };

            mockCarRepo.Setup(repo => repo.GetCarByVIN(It.IsAny<string>()))
                .ReturnsAsync(this.car);

            mockAuctionRepo.Setup(repo => repo.StartAuction(It.IsAny<Auction>()))
                .ReturnsAsync(false);

            await Assert.ThrowsExactlyAsync<CarAlreadyInActiveAuctionException>(async () => await victim.StartAuction(request));
        }
        #endregion

        #region BidOnAuction tests.
        [TestMethod]
        public async Task BidOnAuction_Sucess()
        {
            //var createdAuction = 
            var request = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY1285" , UserId = Guid.NewGuid(), Bid = 20 };
            var auction = new Auction(car);

            mockAuctionRepo.Setup(repo => repo.GetActiveAuctionByCarVIN(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(auction);

            mockAuctionRepo.Setup(repo => repo.BidOnActiveAuction(
                It.Is<string>(s => s.Equals(request.CarVIN)), 
                It.Is<Bid>(s => s.Value == request.Bid && s.UserId.Equals(request.UserId))))
                .ReturnsAsync(true);

            var result = await victim.BidOnAuction(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TimeOfBid);

            Assert.AreEqual(car.VIN, result.AuctionDTO.CarVIN);
            Assert.AreEqual(car.StartingBid, result.AuctionDTO.StartingBid);
            Assert.AreEqual(request.Bid, result.AuctionDTO.LatestBid);
            Assert.IsFalse(result.AuctionDTO.IsCompleted);
        }

        [TestMethod]
        public async Task BidOnAuction_ThrowsActiveAuctionNotFoundException()
        {
            //var createdAuction = 
            var request = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY1285", UserId = Guid.NewGuid(), Bid = 20 };
            Auction auction = null;

            mockAuctionRepo.Setup(repo => repo.GetActiveAuctionByCarVIN(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(auction);

            await Assert.ThrowsExactlyAsync<ActiveAuctionNotFoundException>(async () => await victim.BidOnAuction(request));
        }

        [TestMethod]
        public async Task BidOnAuction_ThrowsLowerThanCurrentHighestBidException()
        {
            //var createdAuction = 
            var request = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY1285", UserId = Guid.NewGuid(), Bid = 20 };
            var auction = new Auction(car);

            mockAuctionRepo.Setup(repo => repo.GetActiveAuctionByCarVIN(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(auction);

            mockAuctionRepo.Setup(repo => repo.BidOnActiveAuction(
                It.Is<string>(s => s.Equals(request.CarVIN)),
                It.Is<Bid>(s => s.Value == request.Bid && s.UserId.Equals(request.UserId))))
                .ReturnsAsync(false);

            await Assert.ThrowsExactlyAsync<LowerThanCurrentHighestBidException>(async () => await victim.BidOnAuction(request));
        }
        #endregion

        #region CloseAuction tests.
        [TestMethod]
        public async Task CloseAuction_Sucess()
        {
            var request = new CloseAuctionRequest { CarVIN = "SET3DKJJFW9KY1285" };
            var auction = new Auction(car);
            auction.CloseAuction();

            mockAuctionRepo.Setup(repo => repo.CloseAuctionByCarVIN(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(auction);

            mockCarRepo.Setup(repo => repo.MarkCarAsUnavailable(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(true);

            var result = await victim.CloseAuction(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.AuctionCloseTime);

            Assert.AreEqual(car.VIN, result.AuctionDTO.CarVIN);
            Assert.IsTrue(result.AuctionDTO.IsCompleted);
        }

        [TestMethod]
        public async Task CloseAuction_ThrowsActiveAuctionNotFoundException()
        {
            var request = new CloseAuctionRequest { CarVIN = "SET3DKJJFW9KY1285" };
            Auction auction = null;

            mockAuctionRepo.Setup(repo => repo.CloseAuctionByCarVIN(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(auction);

            await Assert.ThrowsExactlyAsync<ActiveAuctionNotFoundException>(async () => await victim.CloseAuction(request));
        }

        [TestMethod]
        public async Task CloseAuction_ThrowsMarkCarAsUnavailableException()
        {
            var request = new CloseAuctionRequest { CarVIN = "SET3DKJJFW9KY1285" };
            var auction = new Auction(car);
            auction.CloseAuction();

            mockAuctionRepo.Setup(repo => repo.CloseAuctionByCarVIN(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(auction);

            mockCarRepo.Setup(repo => repo.MarkCarAsUnavailable(It.Is<string>(s => s.Equals(request.CarVIN))))
                .ReturnsAsync(false);

            await Assert.ThrowsExactlyAsync<MarkCarAsUnavailableException>(async () => await victim.CloseAuction(request));
        }
        #endregion

    }
}
