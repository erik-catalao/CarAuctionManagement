using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model.Auctions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuctionManagement.Domain.Model.Cars
{
    public abstract class Car
    {
        public Car(string vin,
            string manufacturer,
            string model,
            int year,
            decimal startingBid,
            CarType carType,
            bool isAvailable) 
        {
            var pattern = @"[A-HJ-NPR-Z0-9]{13}[0-9]{4}";
            if (!Regex.IsMatch(vin, pattern, RegexOptions.IgnoreCase))
                throw new InvalidCarParametersException($"Invalid VIN provided. Current value={vin}");

            if (String.IsNullOrEmpty(manufacturer))
                throw new InvalidCarParametersException($"No car manufacturer has been provided.");

            if (String.IsNullOrEmpty(model))
                throw new InvalidCarParametersException($"No car model has been provided.");

            if (year > DateTime.Now.Year)
                throw new InvalidCarParametersException($"Invalid year provided. Value={year}");

            if (startingBid <= 0)
                throw new InvalidCarParametersException($"Starting bid must be a positive value. Current value={startingBid}");

            VIN = vin;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            CarType = carType;
            IsAvailable = isAvailable;
        }

        public string Manufacturer { get; }
        public string Model { get; }
        public int Year { get; }
        public decimal StartingBid { get; }
        public CarType CarType { get; }
        public string VIN { get; }
        public bool IsAvailable { get; private set; }

        public void MarkAsUnavailable()
        {
            IsAvailable = false;
        }

        public Car Copy()
        {
            return (Car)MemberwiseClone();
        }

        public override bool Equals(object? obj)
        {
            return obj is Car car &&
                   VIN == car.VIN;
        }

        public override int GetHashCode()
        {
            return VIN.GetHashCode();
        }
    }
}
