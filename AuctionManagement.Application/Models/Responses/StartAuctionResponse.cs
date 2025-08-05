using AuctionManagement.Application.Models.DTOs.Auctions;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.Application.Models.Responses
{
    public record StartAuctionResponse
    {
        public required AuctionDTO AuctionDTO { get; set; }
    }
}
