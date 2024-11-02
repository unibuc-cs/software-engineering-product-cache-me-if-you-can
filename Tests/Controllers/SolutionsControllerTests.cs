using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class SolutionsControllerTests
{
    [Fact]
    public void GetAllSolutions_ReturnsViewResult_WithListOfSolutions()
    {
        // Arrange
        var mockRepo = new Mock<ISolutionRepository>();
        mockRepo.Setup(repo => repo.GetAllSolutions())
            .Returns(GetTestSolutions());
        var controller = new SolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllSolutions();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Solution>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 soluții în model
    }

    [Fact]
    public void GetSolutionById_ReturnsViewResult_WithSolution()
    {
        // Arrange
        var mockRepo = new Mock<ISolutionRepository>();
        mockRepo.Setup(repo => repo.GetSolutionById(1))
            .Returns(GetTestSolution());
        var controller = new SolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetSolutionById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Solution>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetSolutionById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<ISolutionRepository>();
        var controller = new SolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetSolutionById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetSolutionById_ReturnsNotFound_WhenSolutionNotFound()
    {
        // Arrange
        var mockRepo = new Mock<ISolutionRepository>();
        mockRepo.Setup(repo => repo.GetSolutionById(1))
            .Returns((Solution)null);
        var controller = new SolutionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetSolutionById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Solution> GetTestSolutions()
    {
        return new List<Solution>
        {
            new Solution { Id = 1, SolutionCode = "Code1", Score = 85, ExerciseId = 1, UserId = "1" },
            new Solution { Id = 2, SolutionCode = "Code2", Score = 90, ExerciseId = 2, UserId = "2" },
        };
    }

    private Solution GetTestSolution()
    {
        return new Solution { Id = 1, SolutionCode = "Code1", Score = 85, ExerciseId = 1, UserId = "1" };
    }
}
