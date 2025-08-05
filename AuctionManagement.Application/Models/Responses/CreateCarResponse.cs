using AuctionManagement.Application.Models.DTOs.Cars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionManagement.Application.Models.Responses
{
    public record CreateCarResponse
    {
        public required CarDTO CarDTO { get; set; }
    }
}
