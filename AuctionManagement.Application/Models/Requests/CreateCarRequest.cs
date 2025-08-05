using AuctionManagement.Application.Models.DTOs.Cars;
using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.Requests
{
    public record CreateCarRequest
    {
        [JsonRequired]
        public required CarDTO Car { get; set; }
    }
}
