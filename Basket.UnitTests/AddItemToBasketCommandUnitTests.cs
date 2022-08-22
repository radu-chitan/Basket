using Basket.Application.Commands;
using Domain.Exceptions;
using FluentAssertions;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Basket.UnitTests
{
    public class AddItemToBasketCommandUnitTests
    {
        private readonly BasketDbContext context = ContextFixture.SetupContext();

        [Theory]
        [InlineData(1, "test-item")]
        public async Task AddItemToBasketCommand_ShouldAddItemToExistingBasket(int id, string item)
        {
            ContextFixture.SeedContext(context);

            //Arrange
            var handler = new AddItemToBasketCommandHandler(context);

            //Act
            await handler.Handle(new AddItemToBasketCommand { BasketId = id, Item = item, Price = 10 }, new CancellationTokenSource().Token);

            //Assert
            var basket = context.Items.FirstOrDefault(t => t.Name == item);
            basket.Should().NotBeNull();
        }

        [Fact]
        public async Task AddItemToBasketCommand_ShouldThrowWhenPriceIsInvalid()
        {
            ContextFixture.SeedContext(context);

            //Arrange
            var handler = new AddItemToBasketCommandHandler(context);

            //Act & Assert
            await Assert.ThrowsAsync<InvalidPriceException>(async () => await handler.Handle(new AddItemToBasketCommand { BasketId = 1, Item = "test-invalid", Price = -1 }, new CancellationTokenSource().Token));
        }
    }
}
