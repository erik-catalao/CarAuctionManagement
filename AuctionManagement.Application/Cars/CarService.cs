using AuctionManagement.Application.Models.Mappers;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Exceptions;
using AuctionManagement.Domain.Model;
using AuctionManagement.Domain.Repositories;

namespace AuctionManagement.Application.Cars
{
    public class CarService(ICarRepository carRepository) : ICarService
    {
        private readonly ICarRepository carRepository = carRepository;

        public async Task<CreateCarResponse> CreateCar(CreateCarRequest createCarRequest)
        {
            var car = CarDtoMapper.MapCreateCarRequest(createCarRequest);

            if (!await carRepository.AddCar(car))
            {
                throw new DuplicateCarException(string.Format("Attempting to add a vehicle that already exists, VIN={0},", car.VIN));
            }

            return new CreateCarResponse { CarDTO = CarDtoMapper.MapDomainToDto(car) };
        }

        public async Task<QueryCarsResponse> QueryCars(CarType? type, string? model, string? manufacturer, int? year)
        {
            var cars = await carRepository.GetAllCars();

            var convertedDtos = cars.Where(c => type != null ? c.CarType.Equals(type) : true)
                .Where(c => model != null ? string.Equals(c.Model, model, StringComparison.OrdinalIgnoreCase) : true)
                .Where(c => manufacturer != null ? string.Equals(c.Manufacturer, manufacturer, StringComparison.OrdinalIgnoreCase) : true)
                .Where(c => year != null ? c.Year.Equals(year) : true)
                .Select(CarDtoMapper.MapDomainToDto);

            return new QueryCarsResponse { CarDTOs = [.. convertedDtos] };
        }
    }
}
