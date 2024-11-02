using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class ExercisesControllerTests
{
    [Fact]
    public void GetAllExercises_ReturnsViewResult_WithListOfExercises()
    {
        // Arrange
        var mockRepo = new Mock<IExerciseRepository>();
        mockRepo.Setup(repo => repo.GetAllExercises())
            .Returns(GetTestExercises());
        var controller = new ExercisesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllExercises();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Exercise>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 exerciții în model
    }

    [Fact]
    public void GetExerciseById_ReturnsViewResult_WithExercise()
    {
        // Arrange
        var mockRepo = new Mock<IExerciseRepository>();
        mockRepo.Setup(repo => repo.GetExerciseById(1))
            .Returns(GetTestExercise());
        var controller = new ExercisesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetExerciseById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Exercise>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetExerciseById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IExerciseRepository>();
        var controller = new ExercisesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetExerciseById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetExerciseById_ReturnsNotFound_WhenExerciseNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IExerciseRepository>();
        mockRepo.Setup(repo => repo.GetExerciseById(1))
            .Returns((Exercise)null);
        var controller = new ExercisesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetExerciseById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Exercise> GetTestExercises()
    {
        return new List<Exercise>
    {
        new Exercise
        {
            Id = 1,
            CategoryId = 1,
            Title = "Test Exercise 1",
            Description = "Description for Test Exercise 1",
            Date = DateTime.Now,
            Summary = "Summary for Test Exercise 1",
            Restrictions = "Restrictions for Test Exercise 1",
            Examples = "Examples for Test Exercise 1",
            Difficulty = "Easy",
            TestCases = "Test cases for Test Exercise 1"
        },
        new Exercise
        {
            Id = 2,
            CategoryId = 2,
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

    private Exercise GetTestExercise()
    {
        return new Exercise
        {
            Id = 1,
            CategoryId = 1,
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