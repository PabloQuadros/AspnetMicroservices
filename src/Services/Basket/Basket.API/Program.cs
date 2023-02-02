using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});


builder.Services.AddScoped<IBasketRepository,BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient> (o => o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")));
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddAutoMapper(typeof(Program));

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
