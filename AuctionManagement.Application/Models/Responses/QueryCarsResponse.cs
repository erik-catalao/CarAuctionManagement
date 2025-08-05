using AuctionManagement.Application.Models.DTOs.Cars;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.Application.Models.Responses
{
    public record QueryCarsResponse
    {
        public required IEnumerable<CarDTO> CarDTOs { get; set; }
    }
}
