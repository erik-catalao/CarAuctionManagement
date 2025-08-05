namespace AuctionManagement.Domain.Exceptions
{
    public class LowerThanCurrentHighestBidException(string message) : Exception(message)
    {
    }
}
