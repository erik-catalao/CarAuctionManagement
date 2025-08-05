using AuctionManagement.Application.Models.DTOs.Auctions;

namespace AuctionManagement.Application.Models.Responses
{
    public record CloseAuctionResponse
    {
        public required AuctionDTO AuctionDTO { get; set; }

        public required DateTime AuctionCloseTime { get; set; }
    }
}
