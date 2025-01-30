using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class WeeklyChallengesControllerTest
{
    [Fact]
    public void GetAllWeeklyChallenges_ReturnsViewResult_WithListOfWeeklyChallenges()
    {
        // Arrange
        var mockRepo = new Mock<IWeeklyChallengeRepository>();
        mockRepo.Setup(repo => repo.GetAllWeeklyChallenges())
            .Returns(GetTestWeeklyChallenges());
        var controller = new WeeklyChallengesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllWeeklyChallenges();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<WeeklyChallenge>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // Check if there are 2 weekly challenges in the model
    }

    [Fact]
    public void GetWeeklyChallengeById_ReturnsViewResult_WithWeeklyChallenge()
    {
        // Arrange
        var mockRepo = new Mock<IWeeklyChallengeRepository>();
        mockRepo.Setup(repo => repo.GetWeeklyChallengeById(1))
            .Returns(GetTestWeeklyChallenge());
        var controller = new WeeklyChallengesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetWeeklyChallengeById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<WeeklyChallenge>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // Check if the ID is correct
    }

    [Fact]
    public void GetWeeklyChallengeById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IWeeklyChallengeRepository>();
        var controller = new WeeklyChallengesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetWeeklyChallengeById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetWeeklyChallengeById_ReturnsNotFound_WhenWeeklyChallengeNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IWeeklyChallengeRepository>();
        mockRepo.Setup(repo => repo.GetWeeklyChallengeById(1))
            .Returns((WeeklyChallenge)null);
        var controller = new WeeklyChallengesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetWeeklyChallengeById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<WeeklyChallenge> GetTestWeeklyChallenges()
    {
        return new List<WeeklyChallenge>
        {
            new WeeklyChallenge { Id = 1, Title = "Test Challenge 1", Description = "Description 1" },
            new WeeklyChallenge { Id = 2, Title = "Test Challenge 2", Description = "Description 2" },
        };
    }

    private WeeklyChallenge GetTestWeeklyChallenge()
    {
        return new WeeklyChallenge { Id = 1, Title = "Test Challenge 1", Description = "Description 1" };
    }
}
