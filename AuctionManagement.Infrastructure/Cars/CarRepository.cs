using AuctionManagement.Domain.Model.Cars;
using AuctionManagement.Domain.Repositories;
using System.Collections.Concurrent;

namespace AuctionManagement.Infrastructure.Cars
{
    public class CarRepository : ICarRepository
    {
        private readonly ConcurrentDictionary<string, Car> CarDatabase;

        public CarRepository()
        {
            CarDatabase = new ConcurrentDictionary<string, Car>();
        }

        public Task<IEnumerable<Car>> GetAllCars()
        {
            IEnumerable<Car> copiedDatabase = new HashSet<Car>(CarDatabase.Values.Select(c => c.Copy()));
            return Task.FromResult(copiedDatabase);
        }

        public Task<bool> AddCar(Car car)
        {
            return Task.FromResult(CarDatabase.TryAdd(car.VIN, car));
        }

        public Task<Car?> GetCarByVIN(string vin)
        {
            var result = CarDatabase.FirstOrDefault(dict => dict.Key == vin);
            return Task.FromResult(result.Value?.Copy());
        }

        public Task<bool> MarkCarAsUnavailable(string vin)
        {
            var car = CarDatabase.FirstOrDefault(dict => dict.Key == vin);

            if (car.Value != null) { 
                car.Value.MarkAsUnavailable();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
