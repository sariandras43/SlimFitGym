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

        public EntriesControllerIntegrationTest(WebApplicationFactory<Program> factory) => this.client = factory.CreateClient();

        [Theory]
        [MemberData(nameof(GetEntriesGetTestData))]
        public async Task GetEntriesShouldReturnOnlyLoggedInPersonsEntriesExpectAtAdminRole(int id, string email, string password, bool success)
        {
            // Arrange
            string request = "/api/entries/" + id;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email,password).Result}");

            // Act
            HttpResponseMessage response = await client.GetAsync(request);

            // Assert
            if (success)
            {
                List<Entry> entries = JsonConvert.DeserializeObject<List<Entry>>(await response.Content.ReadAsStringAsync())!;
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("OK", response.StatusCode.ToString());
                    Assert.IsType<List<Entry>>(entries);
                });
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("Forbidden", response.StatusCode.ToString());
                });
            }
        }

        [Theory]
        [MemberData(nameof(GetEntriesPostTestData))]
        public async Task PostEntryShouldReturnOkIfLoggedInPersonWantsToEntry(int id, string email, string password, bool success)
        {
            // Arrange
            string request = "/api/entries/" + id;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");

            //Entry entryRequest = new Entry()
            //{
            //    Id = 0,
            //    AccountId = id
            //};

            //string jsonContent = JsonConvert.SerializeObject(entryRequest);
            //StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Act
            HttpResponseMessage response = await client.PostAsync(request,null);

            // Assert
            if (success)
            {
                Entry entry = JsonConvert.DeserializeObject<Entry>(await response.Content.ReadAsStringAsync())!;
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("OK", response.StatusCode.ToString());
                    Assert.IsType<Entry>(entry);
                });
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("Forbidden", response.StatusCode.ToString());
                });
            }
        }

        [Fact]
        public async Task PostEntryShouldReturnBadRequestWhenNoMoreEntriesLeftOnThePass()
        {
            // Arrange
            string request = "/api/entries/3";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("pista@gmail.com", "pista").Result}");

            Entry entryRequest = new Entry()
            {
                Id = 0,
                AccountId = 3
            };

            string jsonContent = JsonConvert.SerializeObject(entryRequest);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response= new HttpResponseMessage();
            for (int counter = 0; counter < 30; counter++) 
            {
                response = await client.PostAsync(request, content);
            }

            ErrorModel error = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync())!;
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("BadRequest", response.StatusCode.ToString());
                Assert.IsType<ErrorModel>(error);
                Assert.Equal("Ezzel a bérlettel nem lehet többször belépni.", error.Message);
            });

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetEntriesWithoutLoginShouldReturnUnauthorized(int id)
        {
            // Arrange
            string request = "/api/entries/" + id;

            // Act
            HttpResponseMessage response = await client.GetAsync(request);

            // Assert
            ErrorModel error = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync());
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

        public static List<T> ReadTestData<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json)!.ToList();
        }

        public static IEnumerable<object[]> GetEntriesGetTestData()
        {
            string filePath = "./Data/EntriesGetTestData.json";
            var testCases = ReadTestData<EntryData>(filePath);

            foreach (var testCase in testCases)
            {
                yield return new object[] {testCase.Id ,testCase.Email, testCase.Password, testCase.Success};
            }
        }

        public static IEnumerable<object[]> GetEntriesPostTestData()
        {
            string filePath = "./Data/EntriesPostTestData.json";
            var testCases = ReadTestData<EntryData>(filePath);

            foreach (var testCase in testCases)
            {
                yield return new object[] { testCase.Id, testCase.Email, testCase.Password, testCase.Success };
            }
        }

        class EntryData
        {
            public int Id { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public bool Success { get; set; }
        }
    }
}
