using MassTransit;
using OrderFlowV2.Payment.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x => {

    // 1. Ödeme Consumer'ını tanıtıyoruz:
    x.AddConsumer<StockReservedEventConsumer>();

    x.UsingRabbitMq((context, cfg) => {

        cfg.Host("rabbitmq", "/", h =>
        {
           
            h.Username("guest");
            h.Password("guest");
            
        });

        // 2.Dinlenecek kuyruğu (queue) belirtiyoruz:
        cfg.ReceiveEndpoint("stock-reserved-queue", e =>
        {
            e.ConfigureConsumer<StockReservedEventConsumer>(context);
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
