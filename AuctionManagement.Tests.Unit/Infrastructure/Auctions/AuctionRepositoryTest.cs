using AuctionManagement.Domain.Model.Auctions;
using AuctionManagement.Domain.Model.Cars;
using AuctionManagement.Infrastructure.Auctions;

namespace AuctionManagement.Tests.Unit.Infrastructure.Auctions
{
    [TestClass]
    [TestCategory("Unit")]
    public class AuctionRepositoryTest
    {
        private AuctionRepository victim = new AuctionRepository();

        [TestInitialize]
        public void Startup()
        {
            victim.StartAuction(new Auction(new Hatchback("SET3DKJJFW9KY1281", "Honda", "Accord", 2012, 10.00m, 2)));
            victim.StartAuction(new Auction(new Truck("SET3DKJJFW9KY1282", "Honda", "Accord", 2012, 10.00m, 1500)));
            victim.StartAuction(new Auction(new Sudan("SET3DKJJFW9KY1283", "Honda", "Accord", 2012, 10.00m, 4)));
            victim.StartAuction(new Auction(new SUV("SET3DKJJFW9KY1284", "Honda", "Accord", 2012, 10.00m, 1)));
        }

        #region StartAuction
        [TestMethod]
        public async Task StartAuction_Success()
        {
            var result = await victim.StartAuction(new Auction(new Hatchback("SET3DKJJFW9KY1288", "Honda", "Accord", 2012, 10.00m, 2)));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task StartAuction_Failure()
        {
            var result = await victim.StartAuction(new Auction(new Hatchback("SET3DKJJFW9KY1288", "Honda", "Accord", 2012, 10.00m, 2)));

            Assert.IsTrue(result);
        }
        #endregion

        #region GetActiveAuctionByCarVIN
        [TestMethod]
        public async Task GetActiveAuctionByCarVIN_Success()
        {
            var result = await victim.GetActiveAuctionByCarVIN("SET3DKJJFW9KY1281");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetActiveAuctionByCarVIN_Encapsulation()
        {
            var auction = await victim.GetActiveAuctionByCarVIN("SET3DKJJFW9KY1281");
            auction.CloseAuction();

            var result = await victim.GetActiveAuctionByCarVIN("SET3DKJJFW9KY1281");

            Assert.IsFalse(result.IsCompleted);
        }

        [TestMethod]
        public async Task GetActiveAuctionByCarVIN_Failure()
        {
            var result = await victim.GetActiveAuctionByCarVIN("SET3DKJJFW9KY1220");

            Assert.IsNull(result);
        }
        #endregion

        #region BidOnActiveAuction
        [TestMethod]
        public async Task BidOnActiveAuction_Success()
        {
            Bid bid = new Bid(50.46m, Guid.NewGuid());
            var result = await victim.BidOnActiveAuction("SET3DKJJFW9KY1281", bid);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task BidOnActiveAuction_FailureDueToLowBid()
        {
            Bid bid = new Bid(1.01m, Guid.NewGuid());
            var result = await victim.BidOnActiveAuction("SET3DKJJFW9KY1281", bid);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BidOnActiveAuction_FailureDueToNoAuctionFound()
        {
            Bid bid = new Bid(1.01m, Guid.NewGuid());
            var result = await victim.BidOnActiveAuction("SET3DKJJFW9KY1211", bid);

            Assert.IsFalse(result);
        }
        #endregion

        #region CloseAuctionByCarVIN
        [TestMethod]
        public async Task CloseAuctionByCarVIN_Success()
        {
            var result = await victim.CloseAuctionByCarVIN("SET3DKJJFW9KY1281");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CloseAuctionByCarVIN_FailureDueToNoCarFound()
        {
            var result = await victim.CloseAuctionByCarVIN("SET3DKJJFW9KY1111");

            Assert.IsNull(result);
        }
        #endregion

    }
}
