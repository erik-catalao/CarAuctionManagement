using AuctionManagement.Application.Models.Mappers;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model;
using AuctionManagement.Domain.Model.Auctions;
using AuctionManagement.Domain.Repositories;

namespace AuctionManagement.Application.Auctions
{
    public class AuctionService(IAuctionRepository auctionRepository, ICarRepository carRepository) : IAuctionService
    {
        private readonly IAuctionRepository auctionRepository = auctionRepository;

        private readonly ICarRepository carRepository = carRepository;

        public async Task<StartAuctionResponse> StartAuction(StartAuctionRequest startAuctionRequest)
        {
            var car = await carRepository.GetCarByVIN(startAuctionRequest.VIN) 
                ?? throw new CarNotFoundException($"Failed to start an Auction. Car not found. VIN='{startAuctionRequest.VIN}'");

            if (!car.IsAvailable)
                throw new CarNotAvailableException($"Failed to start an Auction. Car is no longer available. VIN='{startAuctionRequest.VIN}'");

            var auction = new Auction(car);

            var isAuctionStarted = await this.auctionRepository.StartAuction(auction);

            if (isAuctionStarted)
            {
                return new StartAuctionResponse() { AuctionDTO = AuctionDtoMapper.MapDomainToDto(auction) };
            }

            throw new CarAlreadyInActiveAuctionException($"Failure starting a new Auction. There is already an Auction ongoing for CarId={startAuctionRequest.VIN}");
        }

        public async Task<BidOnAuctionResponse> BidOnAuction(BidOnAuctionRequest bidOnAuctionRequest)
        {
            var activeAuction = await this.auctionRepository.GetActiveAuctionByCarVIN(bidOnAuctionRequest.CarVIN)
                ?? throw new ActiveAuctionNotFoundException($"No active Auctions found for Car with Id={bidOnAuctionRequest.CarVIN}.");

            var bid = new Bid(bidOnAuctionRequest.Bid, bidOnAuctionRequest.UserId);

            if (!await this.auctionRepository.BidOnActiveAuction(bidOnAuctionRequest.CarVIN, bid))
            {
                throw new LowerThanCurrentHighestBidException(string.Format("Attempting to bid with a lower value than the current highest bid. " +
                    "Bid={0}", bid.Value));
            }

            return new BidOnAuctionResponse() { AuctionDTO = AuctionDtoMapper.MapDomainToDto(activeAuction, bid.Value), TimeOfBid = DateTime.UtcNow };
        }

        public async Task<CloseAuctionResponse> CloseAuction(CloseAuctionRequest closeAuctionRequest)
        {
            var closeAuctionResult = await this.auctionRepository.CloseAuctionByCarVIN(closeAuctionRequest.CarVIN)
                ?? throw new ActiveAuctionNotFoundException(string.Format("No active Auctions found for Car with Id={0}.", closeAuctionRequest.CarVIN));

            // Critical - This is very problematic. What if it fails to save that car is unavailable after closing the Auction?
            // Normally this is handling by having a scoped Transaction that covers both of the changes made on this method.
            // However, doing that programatically with just C# and a HashSet is over-complicating things, and IMO goes beyond the scope of this exercise.
            // I could create a 'ReopenAuction(string VIN)' method to rollback, but I don't like the concept of it.
            // Docs: https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope?view=net-8.0
            var makeCarUnavailableResult = await this.carRepository.MarkCarAsUnavailable(closeAuctionRequest.CarVIN);
            if (!makeCarUnavailableResult)
                throw new MarkCarAsUnavailableException($"Failed to mark car as unavailable after closing an Auction. VIN={closeAuctionRequest.CarVIN}");

            return new CloseAuctionResponse() { 
                AuctionDTO = AuctionDtoMapper.MapDomainToDto(closeAuctionResult),
                AuctionCloseTime = closeAuctionResult.AuctionEndTime ?? DateTime.UtcNow,
            };
        }        
    }
}
