using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class ActivitiesControllerTests
{
    [Fact]
    public void GetAllActivities_ReturnsViewResult_WithListOfActivities()
    {
        // Arrange
        var mockRepo = new Mock<IActivityRepository>();
        mockRepo.Setup(repo => repo.GetAllActivities())
            .Returns(GetTestActivities());
        var controller = new ActivitiesController(null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllActivities();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Activity>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // Check if there are 2 activities in the model
    }

    [Fact]
    public void GetActivityById_ReturnsViewResult_WithActivity()
    {
        // Arrange
        var mockRepo = new Mock<IActivityRepository>();
        mockRepo.Setup(repo => repo.GetActivityById(1))
            .Returns(GetTestActivity());
        var controller = new ActivitiesController(null, null, mockRepo.Object);

        // Act
        var result = controller.GetActivityById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Activity>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // Check if the ID is correct
    }

    [Fact]
    public void GetActivityById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IActivityRepository>();
        var controller = new ActivitiesController(null, null, mockRepo.Object);

        // Act
        var result = controller.GetActivityById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetActivityById_ReturnsNotFound_WhenActivityNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IActivityRepository>();
        mockRepo.Setup(repo => repo.GetActivityById(1))
            .Returns((Activity)null);
        var controller = new ActivitiesController(null, null, mockRepo.Object);

        // Act
        var result = controller.GetActivityById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Activity> GetTestActivities()
    {
        return new List<Activity>
        {
            new Activity { Id = 1, Description = "Test Activity 1", ReputationPoints = 50, isPracticeRelated = true },
            new Activity { Id = 2, Description = "Test Activity 2", ReputationPoints = 30, isPracticeRelated = false }
        };
    }

    private Activity GetTestActivity()
    {
        return new Activity { Id = 1, Description = "Test Activity 1", ReputationPoints = 50, isPracticeRelated = true };
    }
}
