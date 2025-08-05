using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model.Cars;

namespace AuctionManagement.Domain.Model.Auctions
{
    public class Auction
    {
        public Auction(Car car)
        {
            if (car == null)
                throw new InvalidAuctionParametersException("Failure attempting to create an Auction with no Car.");

            Car = car;
            Bids = new LinkedList<Bid>();
            StartingBid = car.StartingBid;
            Id = Guid.NewGuid();
            AuctionStartTime = DateTime.UtcNow;
        }

        public Car Car { get; }
        private LinkedList<Bid> Bids { get; set; }
        public Guid Id { get; }
        public bool IsCompleted { get; private set; }
        public decimal StartingBid { get; }
        public DateTime AuctionStartTime { get; }
        public DateTime? AuctionEndTime { get; private set; }

        public bool BidOnAuction(Bid bid)
        {
            if (IsHighestBid(bid) && !IsCompleted)
            {
                Bids.AddLast(bid);
                return true;
            }

            return false;
        }

        public IEnumerable<Bid> GetBids()
        {
            return new LinkedList<Bid>(Bids);
        }

        public void CloseAuction()
        {
            AuctionEndTime = DateTime.UtcNow;
            IsCompleted = true;
        }

        public bool IsHighestBid(Bid bid)
        {
            return GetLatestBidValue() < bid.Value;
        }

        public decimal GetLatestBidValue()
        {
            try
            {
                return Bids.Last().Value;
            }
            catch (Exception)
            {
                return StartingBid;
            }
        }

        public Auction Copy()
        {
            Auction other = (Auction)MemberwiseClone();
            other.Bids = (LinkedList<Bid>)GetBids();
            return other;
        }
    }
}
