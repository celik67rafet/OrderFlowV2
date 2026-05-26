using MassTransit;
using OrderFlowV2.Notification.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit( x => {

    // Tek bir consumer sınıfımız var ama içinde 4 farklı interface (mesaj tipi) var
    x.AddConsumer<GlobalEventConsumer>();

    x.UsingRabbitMq(( context, cfg ) =>
    {

        cfg.Host("rabbitmq", "/", h => {

            h.Username("guest");
            h.Password("guest");

        });

        // Tüm bildirimleri tek bir kuyruk üzerinden karşılıyoruz:
        cfg.ReceiveEndpoint("notification-service-queue", e =>
        {
            e.ConfigureConsumer<GlobalEventConsumer>(context);
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
