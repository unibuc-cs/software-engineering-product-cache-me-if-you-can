using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class ReactionsControllerTests
{
    [Fact]
    public void GetAllReactions_ReturnsViewResult_WithListOfReactions()
    {
        // Arrange
        var mockRepo = new Mock<IReactionRepository>();
        mockRepo.Setup(repo => repo.GetAllReactions())
            .Returns(GetTestReactions());
        var controller = new ReactionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllReactions();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Reaction>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 reacții în model
    }

    [Fact]
    public void GetReactionById_ReturnsViewResult_WithReaction()
    {
        // Arrange
        var mockRepo = new Mock<IReactionRepository>();
        mockRepo.Setup(repo => repo.GetReactionById(1))
            .Returns(GetTestReaction());
        var controller = new ReactionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetReactionById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Reaction>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetReactionById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IReactionRepository>();
        var controller = new ReactionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetReactionById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetReactionById_ReturnsNotFound_WhenReactionNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IReactionRepository>();
        mockRepo.Setup(repo => repo.GetReactionById(1))
            .Returns((Reaction)null);
        var controller = new ReactionsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetReactionById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Reaction> GetTestReactions()
    {
        return new List<Reaction>
        {
            new Reaction { Id = 1, UserId = "1", QuestionId = 1 },
            new Reaction { Id = 2, UserId = "2", QuestionId = 2 },
        };
    }

    private Reaction GetTestReaction()
    {
        return new Reaction { Id = 1, UserId = "1", QuestionId = 1 };
    }
}
