using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Cars
{
    public record HatchbackDTO : CarDTO
    {
        [JsonRequired]
        public required int NumberOfDoors { get; set; }
    }
}
