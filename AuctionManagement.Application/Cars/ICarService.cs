using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Model;

namespace AuctionManagement.Application.Cars
{
    public interface ICarService
    {
        Task<QueryCarsResponse> QueryCars(CarType? type, string? model, string? manufacturer, int? year);

        Task<CreateCarResponse> CreateCar(CreateCarRequest car);
    }
}
