using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class QuestionsControllerTests
{
    [Fact]
    public void GetAllQuestions_ReturnsViewResult_WithListOfQuestions()
    {
        // Arrange
        var mockRepo = new Mock<IQuestionRepository>();
        mockRepo.Setup(repo => repo.GetAllQuestions())
            .Returns(GetTestQuestions());
        var controller = new QuestionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllQuestions();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Question>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 întrebări în model
    }

    [Fact]
    public void GetQuestionById_ReturnsViewResult_WithQuestion()
    {
        // Arrange
        var mockRepo = new Mock<IQuestionRepository>();
        mockRepo.Setup(repo => repo.GetQuestionById(1))
            .Returns(GetTestQuestion());
        var controller = new QuestionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetQuestionById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Question>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetQuestionById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IQuestionRepository>();
        var controller = new QuestionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetQuestionById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetQuestionById_ReturnsNotFound_WhenQuestionNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IQuestionRepository>();
        mockRepo.Setup(repo => repo.GetQuestionById(1))
            .Returns((Question)null);
        var controller = new QuestionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetQuestionById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Question> GetTestQuestions()
    {
        return new List<Question>
        {
            new Question { Id = 1, Title = "Test Question 1", Description = "Description 1" },
            new Question { Id = 2, Title = "Test Question 2", Description = "Description 2" },
        };
    }

    private Question GetTestQuestion()
    {
        return new Question { Id = 1, Title = "Test Question 1", Description = "Description 1" };
    }
}
