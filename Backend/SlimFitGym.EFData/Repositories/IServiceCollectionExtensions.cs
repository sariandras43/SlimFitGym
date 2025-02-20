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
            service.AddDbContext<SlimFitGymContext>();
        }

        public static void AddJwtAuthorization(this IServiceCollection service)
        {
            service.AddSingleton<TokenGenerator>();
            service.AddAuthorization();
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    string keyFromConfig = "9a0a8ac154721c7e21f4f1da80a4a19d65f5706af6cf85a0bd8c36b8506263b40221c1b0863df7073bbc61959d971e3e97b80de5f8225201a23fab44b36db9da89fd0b55667cfff43e73f816dbc7505285b16d916bc26f4ed1bae99d41ab135970c34618ddda1f389d6254e2c09e49ccc89e5bd5ab61aeafcf499324c169a3df8b3d8357cd6285212749c66add38f323e971f67bc09e2e575807a4e89199b096e2e81bf21680a14a074ef946cc123a987e13f5a11e9aaec15e0330808337b614139bd83db5d30197bff33c7c236639de5c8e3b4d85482fd03547d0ec2e804f7e82dfbb9f0692a78187c64d27857103f9aaff6ae3a2872a22e6719457a3dda2b5";
                    Byte[] key = System.Text.Encoding.UTF8.GetBytes(keyFromConfig);
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = "Issuer",
                        ValidAudience = "Audience",
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        }
    }
}
