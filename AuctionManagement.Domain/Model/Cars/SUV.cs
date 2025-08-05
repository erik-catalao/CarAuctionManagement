using AuctionManagement.Domain.Exceptions;

namespace AuctionManagement.Domain.Model.Cars
{
    public class SUV : Car
    {
        public SUV(string id,
            string manufacturer,
            string model,
            int year,
            decimal startingBid,
            int numberOfSeats,
            bool isAvailable = true)
            : base(id, manufacturer, model, year, startingBid, CarType.SUV, isAvailable)
        {
            if (numberOfSeats < 1)
                throw new InvalidCarParametersException($"Number of seats must be equal or higher than 1. Current value={numberOfSeats}");

            NumberOfSeats = numberOfSeats;
        }

        public int NumberOfSeats { get; }
    }
}
