using AuctionManagement.Application.Models.DTOs.Auctions;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.Application.Models.Responses
{
    public record BidOnAuctionResponse
    {
        public required AuctionDTO AuctionDTO { get; set; }

        public required DateTime TimeOfBid { get; set; }
    }
}
