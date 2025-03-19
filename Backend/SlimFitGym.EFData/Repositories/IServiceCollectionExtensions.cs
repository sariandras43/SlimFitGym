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
using SlimFitGym.EFData.Interfaces;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;

namespace SlimFitGym.Data.Repository
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<IMachinesRepository, MachinesRepository>();
            service.AddScoped<IRoomsRepository, RoomsRepository>();
            service.AddScoped<IRoomsAndMachinesRepository, RoomsAndMachinesRepository>();
            service.AddScoped<ITrainingsRepository, TrainingsRepository>();
            service.AddScoped<IReservationRepository, ReservationRepository>();
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IPassesRepository, PassesRepository>();
            service.AddScoped<IPurchasesRepository, PurchasesRepository>();
            service.AddScoped<ITrainerApplicantsRepository, TrainerApplicantsRepository>();
            service.AddScoped<IEntriesRepository, EntriesRepository>();
            service.AddScoped<IImagesRepository, ImagesRepository>();
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
        public static void CorsAllowAllOrigins(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder
                        .AllowAnyOrigin() 
                        .AllowAnyMethod() 
                        .AllowAnyHeader(); 
                });
            });
        }

        public static void AddSwaggerWithCustomOptions(this IServiceCollection service) 
        {
            service.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Kérlek adj meg egy JWT tokent!",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
            });
        }

        public static void AddRateLimit(this IServiceCollection service, IConfiguration config)
        {
            service.AddMemoryCache();
            service.AddInMemoryRateLimiting();
            service.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            service.Configure<IpRateLimitOptions>(config);

            service.Configure<IpRateLimitOptions>(options =>
            {
                var generalRules = config.GetSection("IpRateLimiting:GeneralRules").Get<List<RateLimitRule>>();

                options.GeneralRules = generalRules ?? new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 1000,
                        Period = "1m",
                        QuotaExceededResponse = new QuotaExceededResponse(){StatusCode=429,ContentType="application/json", Content="Túl sok kérés."}
                    }
                };
            });
        }
    }
}
