using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using System.Text;

namespace SlimFitGym.Tests.IntegrationTests
{
    public class TrainingsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient client;


        public TrainingsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTrainingsShouldReturnTrainings()
        {
            // Arrange
            var request = "/api/trainings";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var trainings = JsonConvert.DeserializeObject<List<TrainingResponse>>(await response.Content.ReadAsStringAsync());


            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<TrainingResponse>>(trainings);
                Assert.True(trainings.Count > 0);
            });
        }

        [Fact]
        public async Task GetTrainingsWithLimitShouldReturnSpecificAmountOfTrainings()
        {
            // Arrange
            var request = "/api/trainings?limit=5";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var trainings = JsonConvert.DeserializeObject<List<TrainingResponse>>(await response.Content.ReadAsStringAsync());


            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<TrainingResponse>>(trainings);
                Assert.Equal(5,trainings.Count);
            });
        }

        [Fact]
        public async Task GetTrainingsWhichHasZumbaInItsName()
        {
            // Arrange
            var request = "/api/trainings?query=Zumba";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var trainings = JsonConvert.DeserializeObject<List<TrainingResponse>>(await response.Content.ReadAsStringAsync());


            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<TrainingResponse>>(trainings);
                Assert.Single(trainings);
            });
        }

        [Fact]
        public async Task GetAllTrainingsShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/trainings/all";

            // Act
            var response = await client.GetAsync(request);
            
            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task GetAllTrainingsShouldReturnForbiddenWhenLoggedInUserIsNotAdmin()
        {
            // Arrange
            var request = "/api/trainings/all";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await Login("kazmer@gmail.com", "kazmer")}");

            // Act
            var response = await client.GetAsync(request);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task GetAllTrainingsShouldReturnAllTrainingsWhenLoggedInAsAdmin()
        {
            // Arrange
            var request = "/api/trainings/all";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await Login("admin@gmail.com", "admin")}");

            // Act
            var response = await client.GetAsync(request);

            // Assert
            var trainings = JsonConvert.DeserializeObject<List<TrainingResponse>>(await response.Content.ReadAsStringAsync());


            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<List<TrainingResponse>>(trainings);
                Assert.True(trainings.Count > 0);
            });
        }

        [Fact]
        public async Task NewTrainingShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/trainings";

            // Act
            var response = await client.PostAsync(request,null);

            // Assert
            var trainings = JsonConvert.DeserializeObject<List<TrainingResponse>>(await response.Content.ReadAsStringAsync());

            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task DeleteTrainingShouldReturnUnathorizedWhenLoggedOut()
        {
            // Arrange
            var request = "/api/trainings/1";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Unauthorized", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task NewTrainingShouldReturnForbiddenWhenLoggedInUserIsNotTrainer()
        {
            // Arrange
            var request = "/api/trainings";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await Login("pista@gmail.com", "pista")}");

            // Act
            var response = await client.PostAsync(request, null);

            // Assert
            var trainings = JsonConvert.DeserializeObject<List<TrainingResponse>>(await response.Content.ReadAsStringAsync());

            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("Forbidden", response.StatusCode.ToString());
            });
        }

        [Fact]
        public async Task NewTrainingShouldReturnNewTrainingWhenTrainerUploadsATraining()
        {
            // Arrange
            var request = "/api/trainings";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await Login("kazmer@gmail.com", "kazmer")}");


            TrainingRequest newTraining = new TrainingRequest()
            {
                Id = 0,
                MaxPeople = 10,
                Name = "Test",
                RoomId = 1,
                TrainerId = 2,
                TrainingStart = DateTime.UtcNow.AddYears(2),
                TrainingEnd = DateTime.UtcNow.AddYears(2).AddHours(1)
            };
            var jsonContent = JsonConvert.SerializeObject(newTraining);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request, content);

            // Assert
            var training = JsonConvert.DeserializeObject<TrainingResponse>(await response.Content.ReadAsStringAsync());

            Assert.Multiple(() => {
                Assert.NotNull(response);
                Assert.Equal("OK", response.StatusCode.ToString());
                Assert.IsType<TrainingResponse>(training);
                Assert.Equal("Test",training.Name);
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
