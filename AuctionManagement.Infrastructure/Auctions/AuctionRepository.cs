using AuctionManagement.Domain.Model.Auctions;
using AuctionManagement.Domain.Repositories;
using System.Collections.Concurrent;

namespace AuctionManagement.Infrastructure.Auctions
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ConcurrentDictionary<string, object> ThreadLockDict =
                                                    new ConcurrentDictionary<string, object>();

        private Object GenerateLockByCarVIN(string carVIN)
        {
            return ThreadLockDict.GetOrAdd(carVIN, tKey => new Object());

        }

        private HashSet<Auction> AuctionDatabase;

        public AuctionRepository()
        {
            AuctionDatabase = [];
        }

        public Task<bool> StartAuction(Auction auction)
        {
            var lockByCarVIN = this.GenerateLockByCarVIN(auction.Car.VIN);
            lock (lockByCarVIN)
            {
                var activeAuction = GetRealAuctionByCarVIN(auction.Car.VIN);
                if (activeAuction == null)
                {
                    return Task.FromResult(AuctionDatabase.Add(auction));
                }

                return Task.FromResult(false);
            } 
        }

        public Task<bool> BidOnActiveAuction(string carId, Bid bid)
        {
            var lockByCarVIN = this.GenerateLockByCarVIN(carId);
            lock (lockByCarVIN)
            {
                var activeAuction = this.GetRealAuctionByCarVIN(carId);
                var successfulBid = activeAuction?.BidOnAuction(bid);

                return Task.FromResult(successfulBid != null && successfulBid.Value);
            }
        }

        public Task<Auction?> CloseAuctionByCarVIN(string carVIN)
        {
            var lockByCarVIN = this.GenerateLockByCarVIN(carVIN);
            lock (lockByCarVIN)
            {
                var activeAuction = this.GetRealAuctionByCarVIN(carVIN);
                activeAuction?.CloseAuction();

                return Task.FromResult(ReturnAuctionCopy(activeAuction));
            }
        }

        public Task<Auction?> GetActiveAuctionByCarVIN(string carVIN)
        {
            var realAuction = this.GetRealAuctionByCarVIN(carVIN);

            return Task.FromResult(ReturnAuctionCopy(realAuction));
        }

        private Auction? ReturnAuctionCopy(Auction? realAuction)
        {
            if (realAuction != null)
            {
                return realAuction.Copy();
            }
            return realAuction;
        }

        private Auction? GetRealAuctionByCarVIN(string carVIN)
        {
            return AuctionDatabase.FirstOrDefault(a => a.Car.VIN.Equals(carVIN) && !a.IsCompleted);
        }
    }
}
