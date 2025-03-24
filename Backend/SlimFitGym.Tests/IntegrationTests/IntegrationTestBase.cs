using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlimFitGym.EFData;
using SlimFitGymBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Tests.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        protected readonly HttpClient Client;
        protected readonly SlimFitGymContext DbContext;
        private readonly IServiceScope _scope;

        protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
        {
            Client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SlimFitGymContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<SlimFitGymContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDb-{Guid.NewGuid()}");
                    });
                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<SlimFitGymContext>();
                    context.Database.EnsureCreated();
                    SeedTestData(context);
                });
            }).CreateClient();

            _scope = factory.Services.CreateScope();
            DbContext = _scope.ServiceProvider.GetRequiredService<SlimFitGymContext>();
        }

        protected virtual void SeedTestData(SlimFitGymContext context)
        {
           
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            _scope.Dispose();
        }
    }

}
