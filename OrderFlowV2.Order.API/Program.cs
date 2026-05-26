using MassTransit;
using OrderFlowV2.Order.API.Consumers;
using OrderFlowV2.Order.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repository'mizi sisteme kaydediyoruz:
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddMassTransit( x =>
{
    // Önce bu sınıfları MassTransit sistemine kaydet:
    x.AddConsumer<PaymentFailedEventConsumer>();
    x.AddConsumer<StockNotAvailableEventConsumer>();
    x.AddConsumer<PaymentCompletedEventConsumer>();
    x.AddConsumer<OrderShippedEventConsumer>();

    // RabbitMQ yapılandırması:
    x.UsingRabbitMq(( context, cfg ) =>
    {
        cfg.Host( "rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Kuyruk yapılandırması:
        cfg.ReceiveEndpoint("order-payment-failed-queue", e =>
        {
            
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
            
        });

        // Stok yetersizse iptal:
        cfg.ReceiveEndpoint("order-stock-not-available-queue", e => {

            e.ConfigureConsumer<StockNotAvailableEventConsumer>(context);

        });

        // payment completed queue:
        cfg.ReceiveEndpoint("order-payment-completed-queue", e =>
        {
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });

        // shipped için queue:
        cfg.ReceiveEndpoint("order-shiped-queue",e =>
        {
            e.ConfigureConsumer<OrderShippedEventConsumer>(context);
        });

    });

} );

var app = builder.Build();

//Gateway de yazdığımız kural olan sadece /api/orders dan gelen istekleri kabul et için:
app.Use((context, next) =>
{
    context.Request.PathBase = "/order-api";
    return next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
