namespace AuctionManagement.Domain.Exceptions
{
    public class ActiveAuctionNotFoundException(string message) : Exception(message)
    {
    }
}
