using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Cars
{
    public record SuvDTO : CarDTO
    {
        [JsonRequired]
        public required int NumberOfSeats { get; set; }
    }
}
