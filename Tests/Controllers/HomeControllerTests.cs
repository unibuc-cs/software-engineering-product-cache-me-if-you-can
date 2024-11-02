using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Developer_Toolbox.Tests
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;
        private readonly Mock<ILogger<HomeController>> _mockLogger;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Optional: verify the default view is returned
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Optional: verify the default view is returned
        }

    }
}
