using Basket.Application.Queries;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Basket.UnitTests
{
    public class GetBasketItemsQueryHandlerUnitTests
    {
        private GetBasketItemsQueryHandler GetHandler()
        {
            var dbContext = ContextFixture.SetupContext();
            ContextFixture.SeedContext(dbContext);
            return new GetBasketItemsQueryHandler(dbContext);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GettingNonExistingBasket_ThrowsArgumentException(int basketId)
        {
            //Arrange
            var handler = GetHandler();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(new GetBasketItemsQuery { BasketId = basketId }, new CancellationTokenSource().Token));
        }

        [Fact]
        public async Task GettingExistingBasket_ShouldReturnBasketWithItem()
        {
            //Arrange
            var handler = GetHandler();

            //Act
            var result = await handler.Handle(new GetBasketItemsQuery { BasketId = 1 }, new CancellationTokenSource().Token);

            //Assert (hard-coded based on fixture seed)
            result.Should().NotBeNull();
            result.Items.Should().NotBeNullOrEmpty();
            result.TotalNET.Should().Be(10);
            result.TotalGross.Should().Be(11);
        }
    }
}
