using AuctionManagement.Application.Auctions;
using AuctionManagement.Application.Models.Requests;
using AuctionManagement.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuctionController(ILogger<AuctionController> logger, IAuctionService auctionService) : ControllerBase
    {

        private readonly ILogger<AuctionController> _logger = logger;
        private readonly IAuctionService auctionService = auctionService;

        [HttpPost]
        public async Task<ActionResult<StartAuctionResponse>> StartAuction([FromBody] StartAuctionRequest startAuctionRequest)
        {
            _logger.LogInformation($"operation=StartAuction, request={startAuctionRequest}");
            var response = await this.auctionService.StartAuction(startAuctionRequest);

            var location = Url.Action(nameof(StartAuction));
            return Created(location, response);
        }

        [HttpPatch("bids")]
        public async Task<ActionResult<BidOnAuctionResponse>> BidOnAuction([FromBody] BidOnAuctionRequest bidOnAuctionRequest)
        {
            _logger.LogInformation($"operation=BidOnAuction, request={bidOnAuctionRequest}");
            var bidOnAuctionResponse = await this.auctionService.BidOnAuction(bidOnAuctionRequest);

            return Ok(bidOnAuctionResponse);
        }

        [HttpPatch]
        public async Task<ActionResult<CloseAuctionResponse>> CloseAuction([FromBody] CloseAuctionRequest closeAuctionRequest)
        {
            _logger.LogInformation($"operation=CloseAuction, request={closeAuctionRequest}");
            var closeAuctionResponse = await this.auctionService.CloseAuction(closeAuctionRequest);

            return Ok(closeAuctionResponse);
        }
    }
}
