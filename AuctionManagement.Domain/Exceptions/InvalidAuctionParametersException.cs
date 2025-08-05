namespace AuctionManagement.Domain.Exceptions
{
    public class InvalidAuctionParametersException(string message) : Exception(message)
    {
    }
}
