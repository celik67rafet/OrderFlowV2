using MassTransit;
using OrderFlowV2.Shipping.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x => {

    x.AddConsumer<PaymentCompletedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Bu servis "payment-completed-queue" kuyruğunu dinleyecek
        cfg.ReceiveEndpoint("payment-completed-queue", e =>
        {
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
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
