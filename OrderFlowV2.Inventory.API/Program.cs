using MassTransit;
using OrderFlowV2.Inventory.API.Consumers;
using OrderFlowV2.Inventory.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddMassTransit(x =>
{
    // 1. Yazdığımız consumer sınıfını MassTransit'e tanıtıyoruz
    x.AddConsumer<OrderCreatedEventConsumer>();
    x.AddConsumer<PaymentFailedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // 2.RabbitMQ'da hangi kuyruğu (queue) dinleyeceğini belirtiyoruz
        cfg.ReceiveEndpoint("order-created-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });

        // Yeni kuyruk ayarı:
        cfg.ReceiveEndpoint("inventory-payment-failed-queue", e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
