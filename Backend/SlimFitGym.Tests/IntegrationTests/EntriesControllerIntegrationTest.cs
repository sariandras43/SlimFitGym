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
    public class EntriesControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient client;
        private string adminToken;
        private string trainerToken;
        public string userToken { get; set; }

        public EntriesControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            this.client = factory.CreateClient();
            this.adminToken = Login("admin@gmail.com","admin").Result.Token;
            this.trainerToken = Login("kazmer@gmail.com","kazmer").Result.Token;
            this.userToken = Login("pista@gmail.com","pista").Result.Token;

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task AdminCanGetAnyonesEntriesShouldReturnArrayOfEntries(int id)
        {
            // Arrange
            var request = "/api/entries/" + id;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {adminToken}");

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var entries = JsonConvert.DeserializeObject<List<Entry>>(await response.Content.ReadAsStringAsync());

            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<Entry>>(entries);
            });
        }

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TrainerAndUserCantGetEntriesMadeByOthersShouldReturnError(int id)
        {
            // Arrange
            var request = "/api/entries/" + id;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var entries = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync());

            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<Entry>>(entries);
            });
        }
        private async Task<AccountResponse> Login(string email, string password)
        {
            var request = "/api/auth/login";
            LoginRequest loginRequest = new LoginRequest()
            {
                Email = email,
                Password = password,
                RememberMe = false
            };
            var jsonContent = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await client.PostAsync(request, content);

            var login = JsonConvert.DeserializeObject<AccountResponse>(await response.Content.ReadAsStringAsync());
            return login;
        }
    }
}
