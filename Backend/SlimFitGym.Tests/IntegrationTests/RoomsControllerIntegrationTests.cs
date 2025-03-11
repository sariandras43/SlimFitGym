using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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

        [Theory]
        [InlineData("1",true)]
        [InlineData("Nan",false)]
        public async Task GetRoomById(string id, bool success)
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

    }
}
