using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Tests.IntegrationTests
{
    public class MachinesControllerIntegrationTests :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient client;

        public MachinesControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.client = factory.CreateClient();
        }


        [Fact]
        public async Task GetAllMachinesShouldReturnMachineResponseList()
        {
            // Arrange
            var request = "/api/machines"; 

            // Act
            var response = await client.GetAsync(request);
            string json = await response.Content.ReadAsStringAsync();
            List<MachineResponse> content = JsonConvert.DeserializeObject<List<MachineResponse>>(json);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.NotNull(response.Content);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<MachineResponse>>(content);
            });
        }

        [Fact]
        public async Task DeleteMachineShouldReturnUnauthorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/machines/1";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task NewMachineShouldReturnUnauthorizedWhenLoggedOut()
        {
            // Arrange
            MachineRequest newMachine = new MachineRequest()
            {
                Id=0,
                Description="Teszt",
                Name="Test",
                Images = new List<ImageRequest>() { }

            };

            var request = "/api/machines";

            var jsonContent = JsonConvert.SerializeObject(newMachine);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request,content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task PutMachineShouldReturnUnauthorizedWhenLoggedOut()
        {
            // Arrange
            MachineRequest machine = new MachineRequest()
            {
                Id = 1,
                Description = "Teszt",
                Name = "Test",
                Images = new List<ImageRequest>() { }

            };

            var request = "/api/machines";

            var jsonContent = JsonConvert.SerializeObject(machine);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request, content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }
    }
}
