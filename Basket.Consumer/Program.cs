using Basket.Application;
using Persistence;
using Basket.Consumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//TODO: make them run asynchronously so they do not block
builder.Services.AddHostedService<AddItemToBasketConsumerService>();
builder.Services.AddHostedService<BasketCreationConsumerService>();
builder.Services.AddHostedService<CheckoutBasketConsumerService>();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
