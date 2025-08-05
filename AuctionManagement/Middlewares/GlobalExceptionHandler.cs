using AuctionManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (specifiedErrorMessage, statusCode) = GetErrorMessage(exception);

            _logger.LogError(exception, specifiedErrorMessage);

            var problemDetails = new ProblemDetails
            {
                Title = specifiedErrorMessage,
                Status = statusCode,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private Tuple<string, int> GetErrorMessage(Exception exception)
        {
            return exception switch
            {
                InvalidCarParametersException => new("[ERROR]: Failed to create a Car due to invalid parameters.", StatusCodes.Status400BadRequest),
                InvalidAuctionParametersException => new("[ERROR]: Failed to create an Auction due to invalid parameters.", StatusCodes.Status400BadRequest),
                InvalidBidParameterException => new("[ERROR]: Failed to create a Bid due to invalid parameters.", StatusCodes.Status400BadRequest),
                CarNotFoundException => new("[ERROR]: Failed to find a valid Car.", StatusCodes.Status404NotFound),
                ActiveAuctionNotFoundException => new("[ERROR]: Failed to find an active Auction.", StatusCodes.Status404NotFound),
                DuplicateCarException => new("[ERROR]: Failed attempt to add a new Car.", StatusCodes.Status409Conflict),
                CarNotAvailableException => new("[ERROR]: Failed attempt to start a new Auction, due to Car not being available.", StatusCodes.Status409Conflict),
                CarAlreadyInActiveAuctionException => new("[ERROR]: Failed to start a new Auction. There is already an active Auction.", StatusCodes.Status409Conflict),
                LowerThanCurrentHighestBidException => new("[ERROR]: Failed to bid in active Auction. Bid is smaller then current highest bid.", StatusCodes.Status409Conflict),
                MarkCarAsUnavailableException => new("[ERROR]: Failed to mark Car as unavailable after closing an Auction.", StatusCodes.Status500InternalServerError),
                _ => new(exception.Message, StatusCodes.Status500InternalServerError),
            };
        }
    }
}
