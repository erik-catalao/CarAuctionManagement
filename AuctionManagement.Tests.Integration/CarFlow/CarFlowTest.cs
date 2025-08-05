using AuctionManagement.Application.Models.DTOs.Cars;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace AuctionManagement.Tests.Integration.CarFlow
{
    public class CarFlowTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program>
            _factory;

        private JsonSerializerOptions options = new JsonSerializerOptions();

        public CarFlowTest(
            CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            options.Converters.Add(new JsonStringEnumConverter<CarType>());
            options.PropertyNameCaseInsensitive = true;
            options.WriteIndented = true;
        }

        #region CreateCar
        [Fact]
        public async Task Post_CreateCar_Success()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new HatchbackDTO {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY1285",
                    Year = 2012,
                    NumberOfDoors = 4
                }
            };

            var result = await _client.PostAsJsonAsync("/car", createCarRequest);
            var resultResponse = await result.Content.ReadFromJsonAsync<CreateCarResponse>(options);

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(createCarRequest.Car.VIN, resultResponse?.CarDTO.VIN);
            Assert.Equal(typeof(HatchbackDTO), resultResponse?.CarDTO.GetType());
        }

        [Fact]
        public async Task Post_CreateCar_Failure_CarAlreadyExists()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new HatchbackDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY1286",
                    Year = 2012,
                    NumberOfDoors = 4
                }
            };

            var firstCreation = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, firstCreation.StatusCode);

            var result = await _client.PostAsJsonAsync("/car", createCarRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Contains("Attempting to add a vehicle that already exists", resultResponse);
        }
        [Fact]
        public async Task Post_CreateCar_Failure_InvalidCarParameters()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new HatchbackDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "NOOOT_A_VALID_VIIIIIN",
                    Year = 2045,
                    NumberOfDoors = 4
                }
            };

            var result = await _client.PostAsJsonAsync("/car", createCarRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("Failed to create a Car due to invalid parameters", resultResponse);
        }

        #endregion

        #region GetCars
        [Fact]
        public async Task Get_GetCars_ByType()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new HatchbackDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY1211",
                    Year = 2012,
                    NumberOfDoors = 4
                }
            };
            var firstCreation = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, firstCreation.StatusCode);

            var result = await _client.GetAsync("/Car?type=HATCHBACK");
            var resultResponse = await result.Content.ReadFromJsonAsync<QueryCarsResponse>(options);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(resultResponse?.CarDTOs);
            Assert.IsType<HatchbackDTO>(resultResponse.CarDTOs.First());
        }

        [Fact]
        public async Task Get_GetCars_ByYear()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new SudanDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY1222",
                    Year = 1998,
                    NumberOfDoors = 4
                }
            };
            var firstCreation = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, firstCreation.StatusCode);

            var result = await _client.GetAsync("/Car?year=1998");
            var resultResponse = await result.Content.ReadFromJsonAsync<QueryCarsResponse>(options);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(resultResponse?.CarDTOs);
            Assert.IsType<SudanDTO>(resultResponse.CarDTOs.First());
        }

        [Fact]
        public async Task Get_GetCars_ByManufacturer()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new SuvDTO
                {
                    Manufacturer = "Kia",
                    Model = "Picanto",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY1333",
                    Year = 2014,
                    NumberOfSeats = 1
                }
            };
            var firstCreation = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, firstCreation.StatusCode);

            var result = await _client.GetAsync("/Car?manufacturer=Kia");
            var resultResponse = await result.Content.ReadFromJsonAsync<QueryCarsResponse>(options);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(resultResponse?.CarDTOs);
            Assert.IsType<SuvDTO>(resultResponse.CarDTOs.First());
        }

        [Fact]
        public async Task Get_GetCars_ByModel()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Aik",
                    Model = "Secanto",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY1444",
                    Year = 2020,
                    LoadCapacity = 15000
                }
            };
            var firstCreation = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, firstCreation.StatusCode);

            var result = await _client.GetAsync("/Car?model=Secanto");
            var resultResponse = await result.Content.ReadFromJsonAsync<QueryCarsResponse>(options);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(resultResponse?.CarDTOs);
            Assert.IsType<TruckDTO>(resultResponse.CarDTOs.First());
        }
        #endregion
    }
}
