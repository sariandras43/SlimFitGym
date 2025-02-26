using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SlimFitGym.Data.Repository;
using System.Runtime.CompilerServices;
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
            builder.Services.AddJwtAuthorization();
            builder.Services.AddValidationErrorHandler();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //}
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<SlimFitGym.EFData.SlimFitGymContext>();
                //if (context.Database.GetPendingMigrations().Any())
                context.Database.EnsureCreated();
            }
#if DEBUG
            app.Urls.Add("http://*:8080");
#endif
            app.Run();
        }
    }
}
