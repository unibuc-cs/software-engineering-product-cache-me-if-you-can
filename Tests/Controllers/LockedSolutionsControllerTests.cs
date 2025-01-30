using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class LockedSolutionsControllerTests
{
    [Fact]
    public void GetAllLockedSolutions_ReturnsViewResult_WithListOfLockedSolutions()
    {
        // Arrange
        var mockRepo = new Mock<ILockedSolutionRepository>();
        mockRepo.Setup(repo => repo.GetAllLockedSolutions())
            .Returns(GetTestLockedSolutions());
        var controller = new LockedSolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllLockedSolutions();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<LockedSolution>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 soluții în model
    }

    [Fact]
    public void GetSolutionById_ReturnsViewResult_WithSolution()
    {
        // Arrange
        var mockRepo = new Mock<ILockedSolutionRepository>();
        mockRepo.Setup(repo => repo.GetLockedSolutionById(1))
            .Returns(GetTestLockedSolution());
        var controller = new LockedSolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetLockedSolutionById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<LockedSolution>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetSolutionById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<ILockedSolutionRepository>();
        var controller = new LockedSolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetLockedSolutionById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetLockedSolutionById_ReturnsNotFound_WhenLockedSolutionNotFound()
    {
        // Arrange
        var mockRepo = new Mock<ILockedSolutionRepository>();
        mockRepo.Setup(repo => repo.GetLockedSolutionById(1))
            .Returns((LockedSolution)null);
        var controller = new LockedSolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetLockedSolutionById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<LockedSolution> GetTestLockedSolutions()
    {
        return new List<LockedSolution>
        {
            new LockedSolution { Id = 1, SolutionCode = "Code1", Score = 85, LockedExerciseId = 1, UserId = "1" },
            new LockedSolution { Id = 2, SolutionCode = "Code2", Score = 90, LockedExerciseId = 2, UserId = "2" },
        };
    }

    private LockedSolution GetTestLockedSolution()
    {
        return new LockedSolution { Id = 1, SolutionCode = "Code1", Score = 85, LockedExerciseId = 1, UserId = "1" };
    }
}
