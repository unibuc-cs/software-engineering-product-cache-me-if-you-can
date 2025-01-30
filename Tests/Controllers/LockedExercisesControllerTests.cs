using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class LockedExercisesControllerTests
{
    [Fact]
    public void GetAllLockedExercises_ReturnsViewResult_WithListOfLockedExercises()
    {
        // Arrange
        var mockRepo = new Mock<ILockedExerciseRepository>();
        mockRepo.Setup(repo => repo.GetAllLockedExercises())
            .Returns(GetTestLockedExercises());
        var controller = new LockedExercisesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllLockedExercises();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<LockedExercise>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 exerciții în model
    }

    [Fact]
    public void GetLockedExerciseById_ReturnsViewResult_WithLockedExercise()
    {
        // Arrange
        var mockRepo = new Mock<ILockedExerciseRepository>();
        mockRepo.Setup(repo => repo.GetLockedExerciseById(1))
            .Returns(GetTestLockedExercise());
        var controller = new LockedExercisesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetLockedExerciseById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<LockedExercise>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetLockedExerciseById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<ILockedExerciseRepository>();
        var controller = new LockedExercisesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetLockedExerciseById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetLockedExerciseById_ReturnsNotFound_WhenLockedExerciseNotFound()
    {
        // Arrange
        var mockRepo = new Mock<ILockedExerciseRepository>();
        mockRepo.Setup(repo => repo.GetLockedExerciseById(1))
            .Returns((LockedExercise)null);
        var controller = new LockedExercisesController(null, null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetLockedExerciseById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<LockedExercise> GetTestLockedExercises()
    {
        return new List<LockedExercise>
    {
        new LockedExercise
        {
            Id = 1,
            LearningPathId = 1,
            Title = "Test Exercise 1",
            Description = "Description for Test Exercise 1",
            Date = DateTime.Now,
            Summary = "Summary for Test Exercise 1",
            Restrictions = "Restrictions for Test Exercise 1",
            Examples = "Examples for Test Exercise 1",
            Difficulty = "Easy",
            TestCases = "Test cases for Test Exercise 1"
        },
        new LockedExercise
        {
            Id = 2,
            LearningPathId = 2,
            Title = "Test Exercise 2",
            Description = "Description for Test Exercise 2",
            Date = DateTime.Now,
            Summary = "Summary for Test Exercise 2",
            Restrictions = "Restrictions for Test Exercise 2",
            Examples = "Examples for Test Exercise 2",
            Difficulty = "Medium",
            TestCases = "Test cases for Test Exercise 2"
        }
    };
    }

    private LockedExercise GetTestLockedExercise()
    {
        return new LockedExercise
        {
            Id = 1,
            LearningPathId = 1,
            Title = "Test Exercise 1",
            Description = "Description for Test Exercise 1",
            Date = DateTime.Now,
            Summary = "Summary for Test Exercise 1",
            Restrictions = "Restrictions for Test Exercise 1",
            Examples = "Examples for Test Exercise 1",
            Difficulty = "Easy",
            TestCases = "Test cases for Test Exercise 1"
        };
    }

}