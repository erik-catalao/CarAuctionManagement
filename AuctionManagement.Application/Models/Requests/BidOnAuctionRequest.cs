using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.Requests
{
    public record BidOnAuctionRequest
    {
        [JsonRequired]
        [RegularExpression("[A-HJ-NPR-Z0-9]{13}[0-9]{4}", ErrorMessage = "Invalid VIN format.")]
        public required string CarVIN { get; set; }

        [JsonRequired]
        public required Guid UserId { get; set; }

        [JsonRequired]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid must be positive.")]
        public required decimal Bid { get; set; }
    }
}
