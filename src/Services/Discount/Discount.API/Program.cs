using Discount.API.Extensions;
using Discount.API.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Discount.API", Version = "v1" });
});
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();
// app.Services.MigrateDatabase();
// app.MigrateDatabase<WebSocketAcceptContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

app.UseAuthorization();

app.MapControllers();

app.Run();
