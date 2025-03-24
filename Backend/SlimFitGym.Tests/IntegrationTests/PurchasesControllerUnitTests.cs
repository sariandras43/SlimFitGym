using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SlimFitGym.EFData;
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
    public class PurchasesControllerUnitTests : BaseIntegrationTest
    {

        public PurchasesControllerUnitTests(WebApplicationFactory<Program> factory) : base(factory) { }


        [Fact]
        public async Task GetPurchases_ShouldReturnUnathorizedWhenLoggedOut()
        {
            // Act
            var response = await Client.GetAsync("/api/purchases");

            // Assert
            Assert.Equal("Unauthorized",response.StatusCode.ToString());
        }

        [Fact]
        public async Task GetPurchases_ShouldReturnPurchasesWhenLoggedInAsAdmin()
        {
            // Arrange
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("admin@gmail.com", "admin").Result}");

            // Act
            var response = await Client.GetAsync("/api/purchases");

            // Assert
            Assert.Equal("Unauthorized", response.StatusCode.ToString());
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


            HttpResponseMessage response = await Client.PostAsync(request, content);

            AccountResponse login = JsonConvert.DeserializeObject<AccountResponse>(await response.Content.ReadAsStringAsync())!;
            return login.Token;
        }

    }
}
