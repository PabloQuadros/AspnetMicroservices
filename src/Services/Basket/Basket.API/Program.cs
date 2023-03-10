using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using MassTransit.Caching;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Host.UseSerilog(SeriLogger.Configure);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });


        builder.Services.AddScoped<IBasketRepository, BasketRepository>();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddScoped<DiscountGrpcService>();
        builder.Services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
            });
        });

        builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
        builder.Services.AddScoped<DiscountGrpcService>();

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
    }
}