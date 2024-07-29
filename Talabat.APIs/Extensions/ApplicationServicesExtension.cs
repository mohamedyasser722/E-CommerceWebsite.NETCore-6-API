using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Runtime.CompilerServices;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper.AutoMapper;
using Talabat.Core;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, WebApplicationBuilder builder)
        {
            #region Configuration

            


            // sql server Database

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // redis Cache
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetSection("Redis")["ConnectionString"], true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IBasketService, BasketService>();
            Services.AddScoped<IPaymentService, PaymentService>();

            Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var ValidationErrorsResponse = new ApiValidationResponse()
                    {
                        Error = errors
                    };

                    return new BadRequestObjectResult(ValidationErrorsResponse);

                };
            });
            #endregion
            return Services;
        }
    }
}
