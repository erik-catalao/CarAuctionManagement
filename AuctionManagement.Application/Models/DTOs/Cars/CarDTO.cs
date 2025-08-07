using AuctionManagement.Domain.Model;
using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Cars
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(SudanDTO), typeDiscriminator: "sudan")]
    [JsonDerivedType(typeof(HatchbackDTO), typeDiscriminator: "hatchback")]
    [JsonDerivedType(typeof(TruckDTO), typeDiscriminator: "truck")]
    [JsonDerivedType(typeof(SuvDTO), typeDiscriminator: "suv")]
    public record CarDTO
    {
        [JsonRequired]
        public required string Manufacturer { get; set; }
        [JsonRequired]
        public required string Model { get; set; }
        [JsonRequired]
        public required int Year { get; set; }
        [JsonRequired]
        public required decimal StartingBid { get; set; }
        [JsonRequired]
        public required string VIN { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
