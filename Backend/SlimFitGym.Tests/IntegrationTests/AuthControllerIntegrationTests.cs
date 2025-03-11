using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;

namespace SlimFitGym.Tests.IntegrationTests
{
    public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient client;


        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.client = factory.CreateClient();
        }

        public static List<T> ReadTestData<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json)!.ToList();
        }

        public static IEnumerable<object[]> GetRegistrationTestData()
        {
            string filePath = "./Data/RegistrationTestData.json";
            var testCases = ReadTestData<RegistrationRequest>(filePath);

            foreach (var testCase in testCases)
            {
                yield return new object[] { testCase.Email, testCase.Password, testCase.Name, testCase.Phone };
            }
        }

        public static IEnumerable<object[]> GetLoginTestData()
        {
            string filePath = "./Data/LoginTestData.json";
            var testCases = ReadTestData<LoginTestCase>(filePath);

            foreach (var testCase in testCases)
            {
                yield return new object[] { testCase.Email, testCase.Password, testCase.Success };
            }
        }


        [Theory]
        [MemberData(nameof(GetLoginTestData))]
        public async Task LoginWithDifferentAccountInfo(string email, string password, bool success)
        {
            // Arrange
            var request = "/api/auth/login";
            LoginRequest loginRequest = new LoginRequest()
            {
                Email = email,
                Password = password,
                RememberMe = false
            };

            var jsonContent = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request,content);

            // Assert
            if (success)
            {
                var login = JsonConvert.DeserializeObject<AccountResponse>(await response.Content.ReadAsStringAsync());
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("OK", response.StatusCode.ToString());
                    Assert.IsType<AccountResponse>(login);
                    Assert.Equal(email, login.Email);

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
                    Assert.Equal("Helytelen email cím vagy jelszó.", error.Message);
                });
            }
        }

        [Theory]
        [MemberData(nameof(GetRegistrationTestData))]
        public async Task RegistrationWithDifferentBadDataShouldReturnErrorWithBadRequest(string email, string password, string name, string phone)
        {
            // Arrange
            var request = "/api/auth/register";
            RegistrationRequest registrationRequest = new RegistrationRequest()
            {
                Email = email,
                Password = password,
                Name = name,
                Phone=phone
            };

            var jsonContent = JsonConvert.SerializeObject(registrationRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request, content);

            // Assert
            var error = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync());
            Assert.Multiple(() =>
            {
                Assert.NotNull(response);
                Assert.Equal("BadRequest", response.StatusCode.ToString());
                Assert.IsType<ErrorModel>(error);
                Assert.IsType<string>(error.Message);
                Assert.NotEmpty(error.Message);
            });
        }

        public class LoginTestCase()
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool Success { get; set; }
        }

    }
}
