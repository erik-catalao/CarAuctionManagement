using AuctionManagement.Domain.Model.Auctions;

namespace AuctionManagement.Domain.Repositories
{
    public interface IAuctionRepository
    {
        Task<bool> BidOnActiveAuction(string carVIN, Bid bid);
        Task<Auction?> CloseAuctionByCarVIN(string carVIN);
        Task<Auction?> GetActiveAuctionByCarVIN(string carVIN);
        Task<bool> StartAuction(Auction auction);
    }
}
