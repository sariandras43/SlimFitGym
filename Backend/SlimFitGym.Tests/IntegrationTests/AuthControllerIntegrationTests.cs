﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

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
        public async Task LoginWithDifferentAccountInfoShouldReturnErrorOrCredentials(string email, string password, bool success)
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

        [Theory]
        [InlineData("admin@gmail.com","admin",2,false)]
        [InlineData("admin@gmail.com","admin",1,true)]
        [InlineData("admin@gmail.com","admin",3,false)]
        [InlineData("pista@gmail.com","pista",3,true)]
        [InlineData("kazmer@gmail.com","kazmer",3,false)]
        public async Task ModifyAccountDetailsOnlyWorkToLoggedInPerson(string email, string password,int accountIdToModify, bool success)
        {
            // Arrange 
            string request = "/api/auth/modify/" + accountIdToModify;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");

            var requestBody = new {id=accountIdToModify ,name = "Kovács Béla" };

            string jsonContent = JsonConvert.SerializeObject(requestBody);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await client.PutAsync(request, content);

            // Assert
            if (success)
            {
                AccountResponse accountData = JsonConvert.DeserializeObject<AccountResponse>(await response.Content.ReadAsStringAsync())!;

                Assert.NotNull(response);
                Assert.IsType<AccountResponse>(accountData);
                Assert.Equal(email, accountData.Email);
                Assert.Equal("Kovács Béla", accountData.Name);
                Assert.Equal("OK", response.StatusCode.ToString());

            }
            else
            {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            }

        }

        [Theory]
        [InlineData("kazmer@gmail.com", "kazmer", 3, false)]
        [InlineData("pista@gmail.com", "pista", 3, true)]
        public async Task DeleteAccountShouldReturnForbiddenOrDeletesAccount(string email, string password, int accountIdToDelete, bool success)
        {
            // Arrange 
            string request = "/api/auth/delete/" + accountIdToDelete;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");


            // Act
            HttpResponseMessage response = await client.DeleteAsync(request);

            //Assert
            if (success)
            {
                AccountResponse accountData = JsonConvert.DeserializeObject<AccountResponse>(await response.Content.ReadAsStringAsync())!;

                Assert.NotNull(response);
                Assert.IsType<AccountResponse>(accountData);
                Assert.Equal("OK", response.StatusCode.ToString());

            }
            else
            {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            }
        }

        [Theory]
        [InlineData("kazmer@gmail.com", "kazmer")]
        [InlineData("pista@gmail.com", "pista")]
        [InlineData("ica@gmail.com", "ica")]
        public async Task GetAllAccountsSholdReturnForbiddenWhenLoggedInPersonIsNotAdmiin(string email, string password)
        {
            // Arrange 
            string request = "/api/auth/accounts/all" ;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");


            // Act
            HttpResponseMessage response = await client.GetAsync(request);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("Forbidden", response.StatusCode.ToString());
            
        }

        [Fact]
        public async Task DeleteLastAdminAccountShouldReturnError()
        {
            // Arrange 
            string request = "/api/auth/delete/1";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("admin@gmail.com", "admin").Result}");


            // Act
            HttpResponseMessage response = await client.DeleteAsync(request);

            // Assert
            ErrorModel error = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync())!;

            Assert.NotNull(response);
            Assert.Equal("BadRequest", response.StatusCode.ToString());
            Assert.Equal("Utolsó adminisztrátor fiók nem törölhető.", error.Message);
        }

        [Fact]
        public async Task DeleteLastEmployeeShouldReturnError()
        {
            // Arrange 
            string request = "/api/auth/delete/4";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login("ica@gmail.com", "ica").Result}");


            // Act
            HttpResponseMessage response = await client.DeleteAsync(request);

            // Assert
            ErrorModel error = JsonConvert.DeserializeObject<ErrorModel>(await response.Content.ReadAsStringAsync())!;

            Assert.NotNull(response);
            Assert.Equal("BadRequest", response.StatusCode.ToString());
            Assert.Equal("Utolsó dolgozó fiók nem törölhető.", error.Message);
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
        public class LoginTestCase()
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool Success { get; set; }
        }

    }
}
