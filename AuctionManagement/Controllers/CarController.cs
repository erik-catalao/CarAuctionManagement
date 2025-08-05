using AuctionManagement.Application.Cars;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController(ILogger<CarController> logger, ICarService carService) : ControllerBase
    {
        private readonly ILogger<CarController> _logger = logger;
        private readonly ICarService carService = carService;

        [HttpGet(Name = "getCars")]
        public async Task<ActionResult<QueryCarsResponse>> GetCars([FromQuery] CarType? type, 
            [FromQuery] string? model, 
            [FromQuery] string? manufacturer, 
            [FromQuery] int? year)
        {
            _logger.LogInformation($"operation=GetCars, QueryParams => type={type}, model={model}, manufacturer={manufacturer}, year={year}");

            return Ok(await this.carService.QueryCars(type, model, manufacturer, year));
        }

        [HttpPost(Name = "createCar")]
        public async Task<ActionResult<CreateCarResponse>> CreateCar([FromBody] CreateCarRequest createCarRequest)
        {
            _logger.LogInformation($"operation=CreateCar, request={createCarRequest}");
            var result = await this.carService.CreateCar(createCarRequest);

            var location = Url.Action(nameof(CreateCar));
            return Created(location, result);
        }
    }
}
