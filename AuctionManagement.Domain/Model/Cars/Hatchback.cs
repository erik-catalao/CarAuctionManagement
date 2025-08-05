using AuctionManagement.Domain.Exceptions;

namespace AuctionManagement.Domain.Model.Cars
{
    public class Hatchback : Car
    {
        public Hatchback(string id, 
            string manufacturer, 
            string model, 
            int year,
            decimal startingBid,
            int numberOfDoors,
            bool isAvailable = true) 
            : base(id, manufacturer, model, year, startingBid, CarType.HATCHBACK, isAvailable)
        {
            if (numberOfDoors < 2)
                throw new InvalidCarParametersException($"Number of doors must be equal or higher than 2. Current value={numberOfDoors}");

            NumberOfDoors = numberOfDoors;
        }

        public int NumberOfDoors { get; }
    }
}
