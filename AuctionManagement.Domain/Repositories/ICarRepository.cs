using AuctionManagement.Domain.Model.Cars;

namespace AuctionManagement.Domain.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCars();

        Task<bool> AddCar(Car car);

        Task<Car?> GetCarByVIN(string vin);

        Task<bool> MarkCarAsUnavailable(string vin);
    }
}
