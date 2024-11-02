using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class AnswersControllerTests
{
    [Fact]
    public void GetAllAnswers_ReturnsViewResult_WithListOfAnswers()
    {
        // Arrange
        var mockRepo = new Mock<IAnswerRepository>();
        mockRepo.Setup(repo => repo.GetAllAnswers())
            .Returns(GetTestAnswers());
        var controller = new AnswersController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllAnswers();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Answer>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 răspunsuri în model
    }

    [Fact]
    public void GetAnswerById_ReturnsViewResult_WithAnswer()
    {
        // Arrange
        var mockRepo = new Mock<IAnswerRepository>();
        mockRepo.Setup(repo => repo.GetAnswerById(1))
            .Returns(GetTestAnswer());
        var controller = new AnswersController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAnswerById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Answer>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetAnswerById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IAnswerRepository>();
        var controller = new AnswersController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAnswerById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetAnswerById_ReturnsNotFound_WhenAnswerNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IAnswerRepository>();
        mockRepo.Setup(repo => repo.GetAnswerById(1))
            .Returns((Answer)null);
        var controller = new AnswersController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAnswerById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Answer> GetTestAnswers()
    {
        return new List<Answer>
        {
            new Answer { Id = 1, Content = "Test Answer 1", QuestionId = 1 },
            new Answer { Id = 2, Content = "Test Answer 2", QuestionId = 2 }

        };
    }

    private Answer GetTestAnswer()
    {
        return new Answer { Id = 1, Content = "Test Answer 1", QuestionId = 1 };
    }



}
