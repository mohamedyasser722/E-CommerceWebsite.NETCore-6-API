using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helper;
using Talabat.APIs.Helper.AutoMapper;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerServices(); // Extension Method to Add Swagger Services

            #region Application Configuration

            builder.Services.AddApplicationServices(builder);   // Application Services
            builder.Services.AddIdentityServices(builder);      // Identity Services
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins(builder.Configuration["FrontEndUrl"]);
                });
            });                         // CORS Services
            StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:SecretKey"]; // Stripe Configuration

            #endregion

            var app = builder.Build();

            #region Update StoreDbContext.APIs Database and seed data
            using (var scope = app.Services.CreateScope())
            {

                var services = scope.ServiceProvider;
                var _context = services.GetRequiredService<StoreDbContext>();
                var _identityContext = services.GetRequiredService<AppIdentityDbContext>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();

                try
                {
                    var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                        await _context.Database.MigrateAsync();
                        logger.LogInformation("StoreDbContext.APIs Database migration completed successfully.");
                    }
                    else
                    {

                        logger.LogInformation("No pending migrations found.\n\n");
                        logger.LogInformation("StoreDbContext.APIs Database is already up to date.\n\n");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the StoreDbContext.APIs database.");
                }

                try
                {
                    // Await the async call to ensure it completes
                    await StoreDbContextSeed.SeedAsync(_context);
                    logger.LogInformation("StoreDbContext.APIs Database seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the StoreDbContext.APIs database.");
                }
            }
            #endregion

            #region StoreDbContext.Identity Database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var _identityContext = services.GetRequiredService<AppIdentityDbContext>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                try
                {
                    var pendingMigrations = await _identityContext.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                        await _identityContext.Database.MigrateAsync();
                        logger.LogInformation("\n\n\nStoreDbContext.Identity Database migration completed successfully.");
                    }
                    else
                    {

                        logger.LogInformation("No pending migrations found.\n\n");
                        logger.LogInformation("StoreDbContext.Identity Database is already up to date.\n\n");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the StoreDbContext.Identity database.");
                }
                try
                {
                    // Create a UserManager instance
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                    logger.LogInformation("StoreDbContext.Identity Database seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the StoreDbContext.Identity database.");
                }
            } 
            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errorr/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
