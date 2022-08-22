using Basket.Application.Commands;
using FluentAssertions;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Basket.UnitTests
{
    public class CreateBasketCommandHandlerUnitTests
    {
        private readonly BasketDbContext context = ContextFixture.SetupContext();

        [Theory]
        [InlineData("Test_Customer", true)]
        [InlineData("Another_Test_customer", false)]
        public async Task CreateBasketCommand_ShouldCreateBasket(string customer_name, bool pays_vat)
        {
            //Arrange
            ContextFixture.SeedContext(context);
            var handler = new CreateBasketCommandHandler(context);

            //Act
            await handler.Handle(new CreateBasketCommand{ Customer = customer_name, PaysVAT = pays_vat }, new CancellationTokenSource().Token);

            //Assert
            var basket = context.Baskets.FirstOrDefault(t => t.Customer == customer_name);
            basket?.Should().NotBeNull();
            basket?.Customer.Should().Be(customer_name);
            basket?.PaysVAT.Should().Be(pays_vat);
        }
    }
}
