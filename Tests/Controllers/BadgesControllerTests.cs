using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class BadgesControllerTests
{
    [Fact]
    public void GetAllBadges_ReturnsViewResult_WithListOfBadges()
    {
        // Arrange
        var mockRepo = new Mock<IBadgeRepository>();
        mockRepo.Setup(repo => repo.GetAllBadges())
                .Returns(GetTestBadges());
        var controller = new BadgesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllBadges();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Badge>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void GetBadgeById_ReturnsViewResult_WithBadge()
    {
        // Arrange
        var mockRepo = new Mock<IBadgeRepository>();
        mockRepo.Setup(repo => repo.GetBadgeById(1))
                .Returns(GetTestBadge());
        var controller = new BadgesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetBadgeById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Badge>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id);
    }

    [Fact]
    public void GetBadgeById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IBadgeRepository>();
        var controller = new BadgesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetBadgeById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetBadgeById_ReturnsNotFound_WhenBadgeNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IBadgeRepository>();
        mockRepo.Setup(repo => repo.GetBadgeById(1))
                .Returns((Badge)null);
        var controller = new BadgesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetBadgeById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Badge> GetTestBadges()
    {
        return new List<Badge>
        {
            new Badge { Id = 1, Title = "Badge 1", Description = "Description 1", Image = "/img/badge1.png", TargetActivityId = 1 },
            new Badge { Id = 2, Title = "Badge 2", Description = "Description 2", Image = "/img/badge2.png", TargetActivityId = 2 }
        };
    }

    private Badge GetTestBadge()
    {
        return new Badge { Id = 1, Title = "Badge 1", Description = "Description 1", Image = "/img/badge1.png", TargetActivityId = 1 };
    }
}
