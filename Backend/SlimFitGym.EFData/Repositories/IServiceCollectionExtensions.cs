using SlimFitGym.EFData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym.EFData.Repositories;
using SlimFitGymBackend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace SlimFitGym.Data.Repository
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<MachinesRepository>();
            service.AddScoped<RoomsRepository>();
            service.AddScoped<RoomsAndMachinesRepository>();
            service.AddScoped<TrainingsRepository>();
            service.AddScoped<ReservationRepository>();
            service.AddScoped<AccountRepository>();
            service.AddScoped<PassesRepository>();
            service.AddScoped<PurchasesRepository>();
            service.AddScoped<TrainerApplicantsRepository>();
            service.AddScoped<EntriesRepository>();
            service.AddScoped<ImagesRepository>();
            service.AddDbContext<SlimFitGymContext>();
        }

        public static void AddJwtAuthorization(this IServiceCollection service,string privateKey, string issuer, string audience)
        {
            service.AddSingleton<TokenGenerator>();
            service.AddAuthorization();
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    string keyFromConfig = privateKey;
                    Byte[] key = System.Text.Encoding.UTF8.GetBytes(keyFromConfig);
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        }

        public static void AddValidationErrorHandler(this IServiceCollection service)
        {
            service.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errorResponse = new
                        {
                            Message = "Érvénytelen kérés: validációs, típus vagy hiányzó body miatti hiba."
                        };

                        return new BadRequestObjectResult(errorResponse)
                        {
                            ContentTypes = { Application.Json }
                        };
                    };
                });
        }
    }
}
