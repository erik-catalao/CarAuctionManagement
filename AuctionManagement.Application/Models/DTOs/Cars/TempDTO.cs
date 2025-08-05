using System.Text.Json.Serialization;

namespace AuctionManagement.Application.Models.DTOs.Cars
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(TempSudanDto), typeDiscriminator: "sudan")]
    [JsonDerivedType(typeof(TempTruckDto), typeDiscriminator: "truck")]
    [JsonDerivedType(typeof(TempHatchbackDto), typeDiscriminator: "hatchback")]
    [JsonDerivedType(typeof(TempSuvDto), typeDiscriminator: "suv")]
    public record TempDTO
    {
        [JsonRequired]      
        public required string Manufacturer { get; set; }
        [JsonRequired]
        public required int Year { get; set; }
        [JsonRequired]
        public required string Model { get; set; }
    }

    public record TempSudanDto : TempDTO
    {
        public required int NumberOfDoors { get; set; }
    }

    public record TempTruckDto : TempDTO
    {
        public required decimal LoadCapacity { get; set; }
    }

    public record TempHatchbackDto : TempDTO
    {
        public required int NumberOfDoors { get; set; }
    }

    public record TempSuvDto : TempDTO
    {
        public required int NumberOfSeats { get; set; }
    }

}
