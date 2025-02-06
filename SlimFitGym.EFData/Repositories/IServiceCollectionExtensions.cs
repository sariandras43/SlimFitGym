using SlimFitGym.EFData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym.EFData.Repositories;

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
            service.AddDbContext<SlimFitGymContext>();
        }
    }
}
