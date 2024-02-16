using Microsoft.Extensions.Configuration;
using Ordering.Infrastructure;
using Ordering.Application;
using Ordering.API.Extensions;
using Ordering.Infrastructure.Persistence;
using MassTransit;
using EventBus.Messages.Common;
using Ordering.API.EventBusConsumer;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;

// Add services to the container.
// builder.Services.AddApplicationServices();
// builder.Services.AddInfrastructureServices(Configuration);
// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config => {

    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});
// builder.Services.AddMassTransitHostedService();  // https://stackoverflow.com/questions/70187422/addmasstransithostedservice-cannot-be-found

// General Configuration
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<BasketCheckoutConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
// IConfiguration configuration = builder.Configuration;
builder.Services.AddInfrastructureServices(Configuration);


var app = builder.Build();
/* app.MigrateDatabase<WebSocketAcceptContext>();
app.MigrateDatabase<OrderContext>((context, services) =>
  {
      var logger = services.GetService<ILogger<OrderContextSeed>>();
      OrderContextSeed
          .SeedAsync(context, logger)
          .Wait();
  });
*/
/*
builder.Services.AddApplicationServices();
IConfiguration configuration = app.Configuration;
builder.Services.AddInfrastructureServices(configuration);
*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
