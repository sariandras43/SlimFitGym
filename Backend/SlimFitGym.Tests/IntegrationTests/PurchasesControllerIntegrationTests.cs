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
    public class PurchasesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient client;

        public PurchasesControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();
        }


        [Fact]
        public async Task GetPurchasesShouldReturnUnathorizedWhenLoggedOut()
        {
            // Act
            var response = await client.GetAsync("/api/purchases");

            // Assert
            Assert.Equal("Unauthorized",response.StatusCode.ToString());
        }

        [Fact]
        public async Task GetPurchasesShouldReturnForbiddenWhenWhenTryingToGetOtherPersonsPurchasesAsUser()
        {
            //Arrange
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("kazmer@gmail.com", "kazmer").Result}");


            // Act
            var response = await client.GetAsync("/api/purchases/1");

            // Assert
            Assert.Equal("Forbidden", response.StatusCode.ToString());
        }

        [Fact]
        public async Task GetPurchasesShouldReturnPurchasesWhenLoggedInAsAdmin()
        {
            // Arrange
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("admin@gmail.com", "admin").Result}");

            // Act
            var response = await client.GetAsync("/api/purchases");

            // Assert
            List<PurchaseResponse> purchases = JsonConvert.DeserializeObject<List<PurchaseResponse>>(await response.Content.ReadAsStringAsync())!;

            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<PurchaseResponse>>(purchases);
            });
        }

        [Fact]
        public async Task GetPurchasesShouldReturnOwnPurchasesWhenLoggedIn()
        {
            // Arrange
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("pista@gmail.com", "pista").Result}");

            // Act
            var response = await client.GetAsync("/api/purchases/3");

            // Assert
            List<PurchaseResponse> purchases = JsonConvert.DeserializeObject<List<PurchaseResponse>>(await response.Content.ReadAsStringAsync())!;

            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<PurchaseResponse>>(purchases);
            });
        }

        [Fact]
        public async Task NewPurchaseShouldReturnForbiddenWhenLoggedInPersonWantsToCreateAPurchaseToAnotherPerson()
        {
            // Arrange
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("pista@gmail.com", "pista").Result}");
            PurchaseRequest newPurchase = new PurchaseRequest()
            {
                AccountId = 1,
                PassId = 1
            };
            var jsonContent = JsonConvert.SerializeObject(newPurchase);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/purchases",content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task NewPurchaseShouldReturnPurchaseWhenLoggedInPersonWantsToPurchaseToThemself()
        {
            // Arrange
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("pista@gmail.com", "pista").Result}");
            PurchaseRequest newPurchase = new PurchaseRequest()
            {
                AccountId = 3,
                PassId = 2
            };

            var jsonContent = JsonConvert.SerializeObject(newPurchase);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/purchases", content);

            // Assert
            PurchaseResponse purchase = JsonConvert.DeserializeObject<PurchaseResponse>(await response.Content.ReadAsStringAsync())!;
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<PurchaseResponse>(purchase);
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
