using Basket.Application.Queries;
using Basket.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController
    {
        private readonly IMediator mediator;

        public BasketController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<BasketItemsViewModel> GetBasket([FromQuery] int id)
        {
            return await mediator.Send(new GetBasketItemsQuery { BasketId = id });
        }
    }
}
