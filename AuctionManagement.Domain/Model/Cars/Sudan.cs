using AuctionManagement.Domain.Exceptions;

namespace AuctionManagement.Domain.Model.Cars
{
    public class Sudan : Car
    {
        public Sudan(string id,
            string manufacturer,
            string model,
            int year,
            decimal startingBid,
            int numberOfDoors,
            bool isAvailable = true)
            : base(id, manufacturer, model, year, startingBid, CarType.SUDAN, isAvailable)
        {
            if (numberOfDoors < 2)
                throw new InvalidCarParametersException($"Number of doors must be equal or higher than 2. Current value={numberOfDoors}");

            NumberOfDoors = numberOfDoors;
        }

        public int NumberOfDoors { get; }
    }
}
