namespace AuctionManagement.Domain.Exceptions
{
    public class CarNotAvailableException(string message) : Exception(message)
    {
    }
}
