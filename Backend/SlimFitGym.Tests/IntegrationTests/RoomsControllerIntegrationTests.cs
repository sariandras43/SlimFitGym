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
    public class RoomsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient client;
        public RoomsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllRoomsShouldReturnRooms()
        {
            // Arrange
            var request = "/api/rooms";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var rooms = JsonConvert.DeserializeObject<List<RoomAndMachineResponse>>(await response.Content.ReadAsStringAsync());


            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<RoomAndMachineResponse>>(rooms);
                Assert.True(rooms.Count>0);
            });

        }

        [Fact]
        public async Task GetAllRoomsEvenDeletedShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/rooms/all";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });

        }


        [Theory]
        [InlineData("pista@gmail.com", "pista")]
        [InlineData("ica@gmail.com", "ica")]
        [InlineData("kazmer@gmail.com", "kazmer")]
        public async Task GetAllRoomsEvenDeletedShouldReturnForbiddenWhenLoggedinPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            var request = "/api/rooms/all";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");

            // Act
            var response = await client.GetAsync(request);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });

        }

        [Fact]
        public async Task DeleteRoomShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/rooms/1";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });

        }

        [Fact]
        public async Task NewRoomShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/rooms";
            RoomRequest newRoom = new RoomRequest() 
            {
                Name="Teszt",
                Description="Teszt szoba",
                Image = "",
                Machines = new List<MachineForRoom>(),
                RecommendedPeople=10             
            };
            string jsonContent = JsonConvert.SerializeObject(newRoom);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request,content);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });

        }

        [Fact]
        public async Task ModifyRoomShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/rooms/1";
            RoomRequest room = new RoomRequest()
            {
                Id = 1,
                Name = "Teszt",
                Description = "Teszt szoba",
                Image = "",
                Machines = new List<MachineForRoom>(),
                RecommendedPeople = 10
            };
            string jsonContent = JsonConvert.SerializeObject(room);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(request, content);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }

        [Theory]
        [InlineData("pista@gmail.com", "pista")]
        [InlineData("ica@gmail.com", "ica")]
        [InlineData("kazmer@gmail.com", "kazmer")]
        public async Task ModifyRoomShouldReturnForbiddenWhenLoggedInPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            var request = "/api/rooms/1";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");

            RoomRequest room = new RoomRequest()
            {
                Id = 1,
                Name = "Teszt",
                Description = "Teszt szoba",
                Image = "",
                Machines = new List<MachineForRoom>(),
                RecommendedPeople = 10
            };
            string jsonContent = JsonConvert.SerializeObject(room);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(request, content);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });

        }

        [Theory]
        [InlineData("pista@gmail.com","pista")]
        [InlineData("ica@gmail.com","ica")]
        [InlineData("kazmer@gmail.com","kazmer")]
        public async Task NewRoomShouldReturnForbiddenWhenLoggedInPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            var request = "/api/rooms";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");

            RoomRequest newRoom = new RoomRequest()
            {
                Name = "Teszt",
                Description = "Teszt szoba",
                Image = "",
                Machines = new List<MachineForRoom>(),
                RecommendedPeople = 10
            };
            string jsonContent = JsonConvert.SerializeObject(newRoom);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Act
            var response = await client.PostAsync(request, content);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });

        }

        [Theory]
        [InlineData("pista@gmail.com","pista")]
        [InlineData("kazmer@gmail.com","kazmer")]
        [InlineData("ica@gmail.com","ica")]
        public async Task DeleteRoomShoudlReturnForbiddenWhenLoggedInPersonIsNotAdmin(string email, string password)
        {
            // Arrange
            var request = "/api/rooms/1";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Login(email, password).Result}");


            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });

        }

        [Theory]
        [InlineData("1",true)]
        [InlineData("Nan",false)]
        public async Task GetRoomByIdShouldReturnOKOrBadRequestDependingOnTheParameter(string id, bool success)
        {
            // Arrange
            var request = "/api/rooms/" + id;

            // Act
            var response = await client.GetAsync(request);

            // Assert
            if (success)
            {
                var room = JsonConvert.DeserializeObject<RoomAndMachineResponse>(await response.Content.ReadAsStringAsync());
                Assert.Multiple(() =>
                {
                    Assert.NotNull(response);
                    Assert.Equal("OK", response.StatusCode.ToString());
                    Assert.IsType<RoomAndMachineResponse>(room);
                    Assert.Equal(id, room.Id.ToString());

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
                    Assert.Equal("Nem érvényes azonosító.", error.Message);
                });
            }

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
