using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Cars
{
    public record TruckDTO : CarDTO
    {
        [JsonRequired]
        public required int LoadCapacity { get; set; }

    }
}
