using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model.Auctions;

namespace AuctionManagement.Tests.Unit.Domain.Model.Auctions
{
    [TestClass]
    [TestCategory("Unit")]
    public class BidModelTest
    {
        [TestMethod]
        public void CreateValidBid()
        {
            var result = new Bid(10, Guid.NewGuid());

            Assert.IsNotNull(result.UserId);
            Assert.AreEqual(10, result.Value);
            Assert.IsNotNull(result.TimeOfBid);
        }

        [TestMethod]
        public void CreateInvalidBid_ThrowsException()
        {
            Assert.ThrowsExactly<InvalidBidParameterException>(() => new Bid(0, Guid.NewGuid()));
            Assert.ThrowsExactly<InvalidBidParameterException>(() => new Bid(-40, Guid.NewGuid()));
        }
    }
}
