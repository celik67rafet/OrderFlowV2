using MassTransit;
using OrderFlowV2.Basket.API.Consumers;
using OrderFlowV2.Basket.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Kendi yazdığımız servisi kaydediyoruz:
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddMassTransit( x =>
{

    x.AddConsumer<PaymentCompletedEventConsumer>();

    x.UsingRabbitMq((context,cfg) =>
    {

        cfg.Host("rabbitmq", "/", h => {

            h.Username("guest");
            h.Password("guest");

        });

        // sepet ödeme tamamlandı kuyruğunu tanımlıyoruz:
        cfg.ReceiveEndpoint("basket-payment-completed-queue", e =>
        {
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });

    });

} );

// 2. Redis yapılandırmasını ekliyoruz:
builder.Services.AddStackExchangeRedisCache( options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
} );

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
