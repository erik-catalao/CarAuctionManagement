using AuctionManagement.Domain.Exceptions;

namespace AuctionManagement.Domain.Model.Auctions
{
    public class Bid
    {
        public Bid(decimal value, Guid userId)
        {
            if (value <= 0)
                throw new InvalidBidParameterException($"Failure attempting to create a Bid with negative value. Value={value}");

            Value = value;
            UserId = userId;
            TimeOfBid = DateTime.UtcNow;
        }
        public decimal Value { get; }
        public DateTime TimeOfBid { get; }
        public Guid UserId { get; }
    }
}
