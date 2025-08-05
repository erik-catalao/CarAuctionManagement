using AuctionManagement.Application.Models.DTOs.Cars;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using AuctionManagement.Domain.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace AuctionManagement.Tests.Integration.AuctionFlow
{
    public class AuctionFlowTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program>
            _factory;

        private JsonSerializerOptions options = new JsonSerializerOptions();

        public AuctionFlowTest(
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

        #region StartAuction
        [Fact]
        public async Task Post_StartAuction_Success()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY9999",
                    Year = 2012,
                    LoadCapacity = 10000
                }
            };

            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY9999" };

            var createCarForAuction = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, createCarForAuction.StatusCode);

            var result = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            var resultResponse = await result.Content.ReadFromJsonAsync<StartAuctionResponse>(options);
            Assert.Equal(createCarRequest.Car.VIN, resultResponse?.AuctionDTO.CarVIN);
        }

        [Fact]
        public async Task Post_StartAuction_Failure_BadRequest()
        {
            var startAuctionRequest = new StartAuctionRequest { VIN = "NOTAVALIDVIN" };

            var result = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Post_StartAuction_Failure_CarNotFound()
        {
            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY0000" };

            var result = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Failed to find a valid Car", resultResponse);
        }

        [Fact]
        public async Task Post_StartAuction_Failure_CarUnavailable()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY8888",
                    Year = 2012,
                    LoadCapacity = 10000,
                    IsAvailable = false,
                }
            };

            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY8888" };

            var createCarForAuction = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, createCarForAuction.StatusCode);

            var result = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Contains("Failed attempt to start a new Auction, due to Car not being available", resultResponse);
        }

        [Fact]
        public async Task Post_StartAuction_Failure_CarAlreadyInActiveAuction()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY7777",
                    Year = 2012,
                    LoadCapacity = 10000
                }
            };

            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY7777" };

            var createCarForAuction = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, createCarForAuction.StatusCode);

            var createdAuction = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            Assert.Equal(HttpStatusCode.Created, createdAuction.StatusCode);

            var result = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Contains("Failed to start a new Auction. There is already an active Auction", resultResponse);
        }
        #endregion

        #region BidOnAuction
        [Fact]
        public async Task Patch_BidOnAuction_Success()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY6767",
                    Year = 2012,
                    LoadCapacity = 10000
                }
            };

            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY6767" };

            var firstBidOnAuctionRequest = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY6767", Bid = 20.43m, UserId = Guid.NewGuid() };
            var secondBidOnAuctionRequest = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY6767", Bid = 22.43m, UserId = Guid.NewGuid() };

            var createCarForAuction = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, createCarForAuction.StatusCode);

            var createAuctionResult = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            Assert.Equal(HttpStatusCode.Created, createAuctionResult.StatusCode);

            var firstBidResult = await _client.PatchAsJsonAsync("/auction/bids", firstBidOnAuctionRequest);
            Assert.Equal(HttpStatusCode.OK, firstBidResult.StatusCode);

            var firstBidResultResponse = await firstBidResult.Content.ReadFromJsonAsync<BidOnAuctionResponse>(options);
            Assert.Equal(firstBidOnAuctionRequest.Bid, firstBidResultResponse?.AuctionDTO.LatestBid);

            var secondBidResult = await _client.PatchAsJsonAsync("/auction/bids", secondBidOnAuctionRequest);
            Assert.Equal(HttpStatusCode.OK, secondBidResult.StatusCode);
            var secondBidResultResponse = await secondBidResult.Content.ReadFromJsonAsync<BidOnAuctionResponse>(options);
            Assert.Equal(secondBidOnAuctionRequest.Bid, secondBidResultResponse?.AuctionDTO.LatestBid);
        }

        [Fact]
        public async Task Patch_BidOnAuction_Failure_BidLowerThanHighestBid()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY2323",
                    Year = 2012,
                    LoadCapacity = 10000
                }
            };

            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY2323" };

            var firstBidOnAuctionRequest = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY2323", Bid = 20.43m, UserId = Guid.NewGuid() };
            var secondBidOnAuctionRequest = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY2323", Bid = 12.43m, UserId = Guid.NewGuid() };

            var createCarForAuction = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, createCarForAuction.StatusCode);

            var createAuctionResult = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            Assert.Equal(HttpStatusCode.Created, createAuctionResult.StatusCode);

            var firstBidResult = await _client.PatchAsJsonAsync("/auction/bids", firstBidOnAuctionRequest);
            Assert.Equal(HttpStatusCode.OK, firstBidResult.StatusCode);

            var firstBidResultResponse = await firstBidResult.Content.ReadFromJsonAsync<BidOnAuctionResponse>(options);
            Assert.Equal(firstBidOnAuctionRequest.Bid, firstBidResultResponse?.AuctionDTO.LatestBid);

            var result = await _client.PatchAsJsonAsync("/auction/bids", secondBidOnAuctionRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Contains("Failed to bid in active Auction. Bid is smaller then current highest bid", resultResponse);
        }

        [Fact]
        public async Task Patch_BidOnAuction_Failure_NoActiveAuctionFound()
        {
            var bidOnAuctionRequest = new BidOnAuctionRequest { CarVIN = "SET3DKJJFW9KY0000", Bid = 20.43m, UserId = Guid.NewGuid() };

            var result = await _client.PatchAsJsonAsync("/auction/bids", bidOnAuctionRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Failed to find an active Auction", resultResponse);
        }

        #endregion

        #region CloseAuction

        [Fact]
        public async Task Post_CloseAuction_Success()
        {
            var createCarRequest = new CreateCarRequest
            {
                Car = new TruckDTO
                {
                    Manufacturer = "Honda",
                    Model = "Accord",
                    StartingBid = 10.53m,
                    VIN = "SET3DKJJFW9KY4444",
                    Year = 2012,
                    LoadCapacity = 10000
                }
            };

            var startAuctionRequest = new StartAuctionRequest { VIN = "SET3DKJJFW9KY4444" };
            var closeAuctionRequest = new CloseAuctionRequest { CarVIN = "SET3DKJJFW9KY4444" };

            var createCarForAuction = await _client.PostAsJsonAsync("/car", createCarRequest);
            Assert.Equal(HttpStatusCode.Created, createCarForAuction.StatusCode);

            var createAuctionResult = await _client.PostAsJsonAsync("/auction", startAuctionRequest);
            Assert.Equal(HttpStatusCode.Created, createAuctionResult.StatusCode);


            var result = await _client.PatchAsJsonAsync("/auction", closeAuctionRequest);
            var resultResponse = await result.Content.ReadFromJsonAsync<CloseAuctionResponse>(options);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(createCarRequest.Car.VIN, resultResponse?.AuctionDTO.CarVIN);
            Assert.True(resultResponse?.AuctionDTO.IsCompleted);
        }

        [Fact]
        public async Task Post_CloseAuction_Failure_NoActiveAuctionFound()
        {
            var closeAuctionRequest = new CloseAuctionRequest { CarVIN = "SET3DKJJFW9KY0000" };

            var result = await _client.PatchAsJsonAsync("/auction", closeAuctionRequest);
            var resultResponse = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Failed to find an active Auction", resultResponse);
        }

        #endregion
    }
}
