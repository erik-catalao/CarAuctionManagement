using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model.Auctions;
using AuctionManagement.Domain.Model.Cars;

namespace AuctionManagement.Tests.Unit.Domain.Model.Auctions
{
    [TestClass]
    [TestCategory("Unit")]
    public class AuctionModelTest
    {
        [TestMethod]
        public void CreateValidAuction()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);

            var result = new Auction(car);

            Assert.AreEqual(car, result.Car);
            Assert.IsNotNull(result.GetBids());
            Assert.AreEqual(car.StartingBid, result.StartingBid);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.AuctionStartTime);
            Assert.IsNull(result.AuctionEndTime);
        }

        [TestMethod]
        public void CreateInvalidAuction_ThrowsException()
        {
            Assert.ThrowsExactly<InvalidAuctionParametersException>(() => new Auction(null));
        }

        [TestMethod]
        public void BidOnAuction_Success()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            var firstBid = auction.BidOnAuction(new Bid(20, Guid.NewGuid()));
            var secondBid = auction.BidOnAuction(new Bid(30, Guid.NewGuid()));
            var thirdBid = auction.BidOnAuction(new Bid(40, Guid.NewGuid()));

            Assert.IsTrue(firstBid);
            Assert.IsTrue(secondBid);
            Assert.IsTrue(thirdBid);
            Assert.HasCount(3, auction.GetBids());
        }

        [TestMethod]
        public void BidOnAuction_FailureDueToClosedAuction()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            var firstBid = auction.BidOnAuction(new Bid(20, Guid.NewGuid()));
            auction.CloseAuction();
            var secondBid = auction.BidOnAuction(new Bid(30, Guid.NewGuid()));
            var thirdBid = auction.BidOnAuction(new Bid(40, Guid.NewGuid()));

            Assert.IsTrue(firstBid);
            Assert.IsFalse(secondBid);
            Assert.IsFalse(thirdBid);
            Assert.HasCount(1, auction.GetBids());
        }

        [TestMethod]
        public void BidOnAuction_Failure()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            var firstBid = auction.BidOnAuction(new Bid(20, Guid.NewGuid()));
            var secondBid = auction.BidOnAuction(new Bid(15, Guid.NewGuid()));
            var thirdBid = auction.BidOnAuction(new Bid(20, Guid.NewGuid()));

            Assert.IsTrue(firstBid);
            Assert.IsFalse(secondBid);
            Assert.IsFalse(thirdBid);
            Assert.HasCount(1, auction.GetBids());
        }

        [TestMethod]
        public void EnsureBidEncapsulation()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            var firstBid = auction.BidOnAuction(new Bid(20, Guid.NewGuid()));
            var secondBid = auction.BidOnAuction(new Bid(30, Guid.NewGuid()));
            var thirdBid = auction.BidOnAuction(new Bid(40, Guid.NewGuid()));

            LinkedList<Bid> bids = (LinkedList<Bid>)auction.GetBids();

            bids.AddLast(new Bid(50, Guid.NewGuid()));
            bids.AddLast(new Bid(60, Guid.NewGuid()));
            bids.AddLast(new Bid(70, Guid.NewGuid()));

            Assert.HasCount(3, auction.GetBids());
        }


        [TestMethod]
        public void CloseAuction_WithBids()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);
            auction.BidOnAuction(new Bid(20, Guid.NewGuid()));
            auction.BidOnAuction(new Bid(30, Guid.NewGuid()));
            auction.BidOnAuction(new Bid(40, Guid.NewGuid()));

            auction.CloseAuction();

            Assert.IsTrue(auction.IsCompleted);
            Assert.IsNotNull(auction.AuctionEndTime);
            Assert.HasCount(3, auction.GetBids());
        }

        [TestMethod]
        public void CloseAuction_WithoutBids()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            auction.CloseAuction();

            Assert.IsTrue(auction.IsCompleted);
            Assert.IsNotNull(auction.AuctionEndTime);
            Assert.IsEmpty(auction.GetBids());
        }

        [TestMethod]
        public void IsHighestBid()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            var falseResult = auction.IsHighestBid(new Bid(5, Guid.NewGuid()));
            var trueResult = auction.IsHighestBid(new Bid(20, Guid.NewGuid()));

            Assert.IsFalse(falseResult);
            Assert.IsTrue(trueResult);
        }

        [TestMethod]
        public void GetLatestBid()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);

            var resultWithoutBids = auction.GetLatestBidValue();
            auction.BidOnAuction(new Bid(20, Guid.NewGuid()));
            var resultWithBids = auction.GetLatestBidValue();

            Assert.AreEqual(car.StartingBid, resultWithoutBids);
            Assert.AreEqual(20, resultWithBids);
        }

        [TestMethod]
        public void Copy()
        {
            var car = new Hatchback("SET3DKJJFW9KY1285", "Honda", "Accord", 2012, 10, 2);
            var auction = new Auction(car);
            auction.BidOnAuction(new Bid(20, Guid.NewGuid()));

            var copiedAuction = auction.Copy();
            copiedAuction.BidOnAuction(new Bid(30, Guid.NewGuid()));
            copiedAuction.CloseAuction();

            Assert.AreNotEqual(auction.IsCompleted, copiedAuction.IsCompleted);
            Assert.AreNotEqual(auction.GetLatestBidValue(), copiedAuction.GetLatestBidValue());
            Assert.AreNotEqual(auction.GetBids().Count(), copiedAuction.GetBids().Count());
        }
    }
}
