using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.Requests
{
    public record StartAuctionRequest
    {
        [JsonRequired]
        [RegularExpression("[A-HJ-NPR-Z0-9]{13}[0-9]{4}", ErrorMessage = "Invalid VIN format.")]
        public required string VIN { get; set; }
    }
}
