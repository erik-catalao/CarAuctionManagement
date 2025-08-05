using AuctionManagement.Application.Models.DTOs.Auctions;
using AuctionManagement.Domain.Model.Auctions;

namespace AuctionManagement.Application.Models.Mappers
{
    internal class AuctionDtoMapper
    {
        internal static AuctionDTO MapDomainToDto(Auction auction, decimal? latestBid = null)
        {
            var latestBidValue = latestBid ?? auction.GetLatestBidValue();

            return new AuctionDTO()
            {
                Id = auction.Id,
                CarVIN = auction.Car.VIN,
                IsCompleted = auction.IsCompleted,
                AuctionStartTime = auction.AuctionStartTime,
                StartingBid = auction.StartingBid,
                LatestBid = latestBidValue
            };
        }
    }
}
