using AuctionManagement.Application.Models.DTOs.Cars;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model;
using AuctionManagement.Domain.Model.Cars;

namespace AuctionManagement.Application.Models.Mappers
{
    internal class CarDtoMapper
    {
        internal static Car MapCreateCarRequest(CreateCarRequest request)
        {
            return request.Car switch
            {
                HatchbackDTO req => new Hatchback(req.VIN,
                                        req.Manufacturer,
                                        req.Model,
                                        req.Year,
                                        req.StartingBid,
                                        req.NumberOfDoors,
                                        req.IsAvailable ?? true),
                SudanDTO req => new Sudan(req.VIN,
                                        req.Manufacturer,
                                        req.Model,
                                        req.Year,
                                        req.StartingBid,
                                        req.NumberOfDoors,
                                        req.IsAvailable ?? true),
                TruckDTO req => new Truck(req.VIN,
                                        req.Manufacturer,
                                        req.Model,
                                        req.Year,
                                        req.StartingBid,
                                        req.LoadCapacity,
                                        req.IsAvailable ?? true),
                SuvDTO req => new SUV(req.VIN,
                                        req.Manufacturer,
                                        req.Model,
                                        req.Year,
                                        req.StartingBid,
                                        req.NumberOfSeats,
                                        req.IsAvailable ?? true),
                _ => throw new InvalidCarParametersException("Error mapping CreateCarRequest to the Car domain. No '$type' was specified."),
            };
        }

        internal static CarDTO MapDomainToDto(Car originalCar)
        {
            switch (originalCar)
            {
                case Sudan car:
                    return new SudanDTO
                    {
                        VIN = car.VIN,
                        Manufacturer = car.Manufacturer,
                        Year = car.Year,
                        StartingBid = car.StartingBid,
                        Model = car.Model,
                        NumberOfDoors = car.NumberOfDoors,
                        IsAvailable = car.IsAvailable
                    };

                case Hatchback car:
                    return new HatchbackDTO
                    {
                        VIN = car.VIN,
                        Manufacturer = car.Manufacturer,
                        Year = car.Year,
                        StartingBid = car.StartingBid,
                        Model = car.Model,
                        NumberOfDoors = car.NumberOfDoors,
                        IsAvailable = car.IsAvailable
                    };

                case SUV car:
                    return new SuvDTO
                    {
                        VIN = car.VIN,
                        Manufacturer = car.Manufacturer,
                        Year = car.Year,
                        StartingBid = car.StartingBid,
                        Model = car.Model,
                        NumberOfSeats = car.NumberOfSeats,
                        IsAvailable = car.IsAvailable
                    };

                case Truck car:
                    return new TruckDTO
                    {
                        VIN = car.VIN,
                        Manufacturer = car.Manufacturer,
                        Year = car.Year,
                        StartingBid = car.StartingBid,
                        Model = car.Model,
                        LoadCapacity = car.LoadCapacity,
                        IsAvailable = car.IsAvailable
                    };

                default:
                    throw new Exception("Error mapping Car domain to DTO.");
            }
        }

    }
}
