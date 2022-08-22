using Basket.Application.Commands;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Basket.Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IProducerService producerService;
        private readonly string server;
        public BasketController(IMediator mediator, IProducerService producerService, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.producerService = producerService;
            server = configuration.GetSection("Settings").GetValue<string>("BootstrapServer");
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] int id)
        {
            return Ok(await mediator.Send(new GetBasketItemsQuery { BasketId = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasketCommand createBasketCommand)
        {
            string message = JsonSerializer.Serialize(createBasketCommand);
            return Ok(await producerService.SendOrderRequest(Domain.Common.Constants.BASKET_CREATE_TOPIC, message, server));
        }

        [HttpPut]
        [Route("{id}/article-line")]
        public async Task<IActionResult> AddItemToBasket(int id, [FromBody] AddItemToBasketCommand addItemToBasketCommand)
        {
            addItemToBasketCommand.BasketId = id;
            string message = JsonSerializer.Serialize(addItemToBasketCommand);
            return Ok(await producerService.SendOrderRequest(Domain.Common.Constants.BASKET_ADD_ITEM_TOPIC, message, server));
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> CheckoutBasket(int id)
        {
            string message = JsonSerializer.Serialize(new CheckoutBasketCommand { BasketId = id });
            return Ok(await producerService.SendOrderRequest(Domain.Common.Constants.BASKET_CHECKOUT, message, server));
        }
    }
}