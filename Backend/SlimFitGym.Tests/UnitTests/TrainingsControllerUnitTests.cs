using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.EFData.Repositories;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using SlimFitGymBackend.Controllers;

namespace SlimFitGym.Tests.UnitTests
{
    public class TrainingsControllerUnitTests
    {
        private readonly TrainingsController controller;
        private readonly Mock<ITrainingsRepository> trainingsRepositoryMock = new Mock<ITrainingsRepository>();
        private readonly Mock<IReservationRepository> reservationRepositoryMock = new Mock<IReservationRepository>();


        public TrainingsControllerUnitTests() 
        {
            controller = new TrainingsController(trainingsRepositoryMock.Object, reservationRepositoryMock.Object);
        }
        [Fact]
        public void GetAllActiveTrainingsShouldReturnTrainingResponseList()
        {
            // Arrange
            List<TrainingResponse> mockReturnValue = new List<TrainingResponse>()
            {
                new TrainingResponse()
                {
                    Id = 1,
                    FreePlaces = 1,
                    IsActive = true,
                    MaxPeople = 1,
                    Name = "TRX edzés",
                    RoomId = 1,
                    TrainingEnd = DateTime.Now.AddHours(1),
                    TrainingStart = DateTime.Now,
                    RoomImageUrl = "",
                    TrainerId = 1,
                    TrainerImageUrl=""
                }
            };
            trainingsRepositoryMock.Setup(r => r.GetActiveTrainings()).Returns(mockReturnValue);

            // Act
            IActionResult result = controller.Get();
            List<TrainingResponse> returnedData = (result as ObjectResult).Value as List<TrainingResponse>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(mockReturnValue, returnedData);
                Assert.IsType<List<TrainingResponse>>(returnedData);
                Assert.Single(returnedData!);

            });

        }

        [Fact]
        public void DeleteTrainingWhenGivenAValidId()
        {
            // Arrange
            TrainingResponse mockReturnValue = new TrainingResponse()
            {
                Id = 1,
                FreePlaces = 1,
                IsActive = true,
                MaxPeople = 1,
                Name = "TRX edzés",
                RoomId = 1,
                TrainingEnd = DateTime.Now.AddHours(1),
                TrainingStart = DateTime.Now,
                RoomImageUrl = "",
                TrainerId = 1,
                TrainerImageUrl = ""
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            trainingsRepositoryMock.Setup(r => r.DeleteOrMakeInactive("",1)).Returns(mockReturnValue);

            var headers = new HeaderDictionary();

            mockRequest.Setup(r => r.Headers).Returns(headers);
            mockHttpContext.Setup(ctx => ctx.Request).Returns(mockRequest.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            IActionResult result = controller.Delete("1");
            var statuscode = (result as ObjectResult)!.StatusCode;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(200, statuscode);
            });

        }

        [Fact]
        public void DontDeleteTrainingWhenGivenAnInvalidId()
        {
            // Arrange
            TrainingResponse mockReturnValue = new TrainingResponse()
            {
                Id = 1,
                FreePlaces = 1,
                IsActive = true,
                MaxPeople = 1,
                Name = "TRX edzés",
                RoomId = 1,
                TrainingEnd = DateTime.Now.AddHours(1),
                TrainingStart = DateTime.Now,
                RoomImageUrl = "",
                TrainerId = 1,
                TrainerImageUrl = ""
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();

            var headers = new HeaderDictionary();

            mockRequest.Setup(r => r.Headers).Returns(headers);
            mockHttpContext.Setup(ctx => ctx.Request).Returns(mockRequest.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            IActionResult result = controller.Delete("Test");
            var statuscode = (result as ObjectResult)!.StatusCode;
            var message = (result as ObjectResult)!.Value;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(400, statuscode);
            });

        }
    }
}