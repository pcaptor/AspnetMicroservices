using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString"); // "localhost=6379";
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(typeof(Program));

// Grpc Configuration
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<DiscountGrpcService>();

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(Configuration["EventBusSettings:HostAddress"]);
    });
});
// builder.Services.AddMassTransitHostedService();  // https://stackoverflow.com/questions/70187422/addmasstransithostedservice-cannot-be-found

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
