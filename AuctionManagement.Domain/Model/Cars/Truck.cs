using AuctionManagement.Domain.Exceptions;

namespace AuctionManagement.Domain.Model.Cars
{
    public class Truck : Car
    {
        public Truck(string id, 
            string manufacturer, 
            string model, 
            int year, 
            decimal startingBid,
            int loadCapacity,
            bool isAvailable = true)
            : base(id, manufacturer, model, year, startingBid, CarType.TRUCK, isAvailable)
        {
            if (loadCapacity <= 0 )
                throw new InvalidCarParametersException($"Load Capacity must be higher than 0. Current value={loadCapacity}");

            LoadCapacity = loadCapacity;
        }

        public int LoadCapacity { get; }
    }
}
