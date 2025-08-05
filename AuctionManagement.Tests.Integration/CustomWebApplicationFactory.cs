using AuctionManagement.Application.Auctions;
using AuctionManagement.Application.Cars;
using AuctionManagement.Domain.Repositories;
using AuctionManagement.Infrastructure.Auctions;
using AuctionManagement.Infrastructure.Cars;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuctionManagement.Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(CarService));
                services.RemoveAll(typeof(AuctionService));
                services.RemoveAll(typeof(CarRepository));
                services.RemoveAll(typeof(AuctionRepository));

                // Inject services
                services.AddScoped<ICarService, CarService>();
                services.AddScoped<IAuctionService, AuctionService>();

                // Inject repository.
                services.AddSingleton<ICarRepository, CarRepository>();
                services.AddSingleton<IAuctionRepository, AuctionRepository>();
            });

            builder.UseEnvironment("Development");
        }
    }
}
