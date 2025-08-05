using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;

namespace AuctionManagement.Application.Auctions
{
    public interface IAuctionService
    {
        Task<StartAuctionResponse> StartAuction(StartAuctionRequest startAuctionRequest);

        Task<BidOnAuctionResponse> BidOnAuction(BidOnAuctionRequest bidOnAuctionRequest);

        Task<CloseAuctionResponse> CloseAuction(CloseAuctionRequest closeAuctionRequest);
    }
}
