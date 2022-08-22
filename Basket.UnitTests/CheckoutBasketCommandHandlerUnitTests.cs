using Basket.Application.Commands;
using FluentAssertions;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Basket.UnitTests
{
    public class CheckoutBasketCommandHandlerUnitTests
    {
        private readonly BasketDbContext context = ContextFixture.SetupContext();

        [Theory]
        [InlineData(1)]
        public async Task CheckoutBasketCommand_ShouldDeleteExistingBasket(int id)
        {
            ContextFixture.SeedContext(context);

            //Arrange
            var handler = new CheckoutBasketCommandHandler(context);

            //Assert pre deletion
            var existingBasket = context.Baskets.FirstOrDefault(t => t.Id == id);
            existingBasket.Should().NotBeNull();

            //Act
            await handler.Handle(new CheckoutBasketCommand { BasketId = id}, new CancellationTokenSource().Token);

            //Assert
            var basket = context.Baskets.FirstOrDefault(t => t.Id == id);
            basket.Should().BeNull();
        }
    }
}
