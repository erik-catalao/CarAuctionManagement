namespace AuctionManagement.Domain.Exceptions
{
    public class CarAlreadyInActiveAuctionException(string message) : Exception(message)
    {
    }
}
