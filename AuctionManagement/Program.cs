using AuctionManagement.Application.Auctions;
using AuctionManagement.Application.Cars;
using AuctionManagement.Domain.Repositories;
using AuctionManagement.Infrastructure.Auctions;
using AuctionManagement.Infrastructure.Cars;
using AuctionManagement.Middlewares;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Inject services.
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();

// Inject the repos.
builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddLogging();


builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();


app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Added for Integration Testing
public partial class Program { }