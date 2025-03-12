using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Tests.UnitTests
{
    public class MachinesControllerUnitTests
    {
        private readonly MachinesController controller;
        private readonly Mock<IMachinesRepository> machinesRepositoryMock = new Mock<IMachinesRepository>();

        public MachinesControllerUnitTests()
        {
            this.controller = new MachinesController(machinesRepositoryMock.Object);
        }

        [Fact]
        public void GetAllMachinesShouldReturnMachineResponseList()
        {
            // Arrange
            List<MachineResponse> mockReturnValue = new List<MachineResponse>()
            {
                new MachineResponse()
                {
                    Id = 1,
                    Description = "Test",
                    ImageUrls= new List<string>(),
                    Name="TRX szett"
                }
            };
            machinesRepositoryMock.Setup(m => m.GetAllMachine()).Returns(mockReturnValue);

            // Act
            IActionResult result = controller.Get();
            List<MachineResponse> returnedData = (result as ObjectResult).Value as List<MachineResponse>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(mockReturnValue, returnedData);
                Assert.IsType<List<MachineResponse>>(returnedData);
                Assert.Single(returnedData!);

            });

        }

        [Fact]
        public void DeleteMachineWhenGivenAValidId()
        {
            // Arrange
            Machine mockReturnValue = new Machine()
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            machinesRepositoryMock.Setup(m => m.DeleteMachine(1)).Returns(mockReturnValue);

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
        public void DontDeleteMachineWhenGivenAnInvalidId()
        {
            // Arrange
            Machine mockReturnValue = new Machine()
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
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
