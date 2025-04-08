using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SlimFitGym.Models.Models;
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
            List<MachineResponse> machines = JsonConvert.DeserializeObject<List<MachineResponse>>(json);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.NotNull(response.Content);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<MachineResponse>>(machines);
                Assert.True(machines.Count > 0);
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
                Images = new List<string>() { }

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

        [Theory]
        [InlineData("pista@gmail.com", "pista")]
        [InlineData("ica@gmail.com", "ica")]
        [InlineData("kazmer@gmail.com", "kazmer")]
        public async Task NewMachineShouldReturnForbiddenWhenLoggedInPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            MachineRequest newMachine = new MachineRequest()
            {
                Id = 0,
                Description = "Teszt",
                Name = "Test",
                Images = new List<string>() { }

            };

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");
            var request = "/api/machines";

            var jsonContent = JsonConvert.SerializeObject(newMachine);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request, content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });
        }

        [Theory]
        [InlineData("pista@gmail.com", "pista")]
        [InlineData("ica@gmail.com", "ica")]
        [InlineData("kazmer@gmail.com", "kazmer")]
        public async Task ModifyMachineShouldReturnForbiddenWhenLoggedInPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            MachineRequest newMachine = new MachineRequest()
            {
                Id = 1,
                Description = "Teszt",
                Name = "Test",
                Images = new List<string>() { }

            };

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");
            var request = "/api/machines/1";

            var jsonContent = JsonConvert.SerializeObject(newMachine);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(request, content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });
        }

        [Theory]
        [InlineData("pista@gmail.com", "pista")]
        [InlineData("ica@gmail.com", "ica")]
        [InlineData("kazmer@gmail.com", "kazmer")]
        public async Task DeleteMachineShouldReturnForbiddenWhenLoggedInPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");
            var request = "/api/machines/1";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("Nan", false)]
        public async Task GetMachineByIdShouldReturnOKOrBadRequestDependingOnTheParameter(string id, bool success)
        {
            // Arrange
            var request = "/api/machines/" + id;

            // Act
            var response = await client.GetAsync(request);

            // Assert
            if (success)
            {
                var machine = JsonConvert.DeserializeObject<Machine>(await response.Content.ReadAsStringAsync());
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("OK", response.StatusCode.ToString());
                    Assert.IsType<Machine>(machine);
                    Assert.Equal(id, machine.Id.ToString());

                });
            }
            else
            {
                var error = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync());
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("BadRequest", response.StatusCode.ToString());
                    Assert.IsType<ErrorModel>(error);
                    Assert.Equal("Érvénytelen azonosító.", error.Message);
                });
            }

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
                Images = new List<string>() { }

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

        private async Task<string> Login(string email, string password)
        {
            string request = "/api/auth/login";
            LoginRequest loginRequest = new LoginRequest()
            {
                Email = email,
                Password = password,
                RememberMe = false
            };
            string jsonContent = JsonConvert.SerializeObject(loginRequest);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await client.PostAsync(request, content);

            AccountResponse login = JsonConvert.DeserializeObject<AccountResponse>(await response.Content.ReadAsStringAsync())!;
            return login.Token;
        }
    }
}
