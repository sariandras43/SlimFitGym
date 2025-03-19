using AspNetCoreRateLimit;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SlimFitGym.Data.Repository;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;
using static System.Net.Mime.MediaTypeNames;

namespace SlimFitGymBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddRepositories();
            builder.Services.AddJwtAuthorization(builder.Configuration["Auth:Key"]!, builder.Configuration["Auth:Issuer"]!, builder.Configuration["Auth:Audience"]!);
            builder.Services.AddValidationErrorHandler();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.CorsAllowAllOrigins();
            builder.Services.AddSwaggerWithCustomOptions();
            builder.Services.AddRateLimit(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAllOrigins");
            app.UseAuthorization();
            app.UseIpRateLimiting();

            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<SlimFitGym.EFData.SlimFitGymContext>();
                //TODO: removing when we dont need it anymore
                context.Database.EnsureDeleted();
                //
                context.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}
