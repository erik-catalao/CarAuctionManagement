namespace AuctionManagement.Domain.Exceptions
{
    public class InvalidBidParameterException(string message) : Exception(message)
    {
    }
}
