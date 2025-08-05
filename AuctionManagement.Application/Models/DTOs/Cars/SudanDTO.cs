using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Cars
{
    public record SudanDTO : CarDTO
    {
        [JsonRequired]
        public required int NumberOfDoors { get; set; }
    }
}
