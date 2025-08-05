using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Auctions
{
    public record AuctionDTO
    {
        [JsonRequired]
        public required Guid Id { get; set; }
        [JsonRequired]
        public required bool IsCompleted { get; set; }
        [JsonRequired]
        public required DateTime AuctionStartTime { get; set; }
        [JsonRequired]
        public required decimal StartingBid { get; set; }
        [JsonRequired]
        public required decimal LatestBid { get; set; }
        [JsonRequired]
        [RegularExpression("[A-HJ-NPR-Z0-9]{13}[0-9]{4}", ErrorMessage = "Invalid VIN format.")]
        public required string CarVIN { get; set; }
    }
}
