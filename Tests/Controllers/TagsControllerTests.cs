using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class TagsControllerTests
{
    [Fact]
    public void GetAllTags_ReturnsViewResult_WithListOfTags()
    {
        // Arrange
        var mockRepo = new Mock<ITagRepository>();
        mockRepo.Setup(repo => repo.GetAllTags())
            .Returns(GetTestTags());
        var controller = new TagsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllTags();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Tag>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); 
    }

    [Fact]
    public void GetTagById_ReturnsViewResult_WithTag()
    {
        // Arrange
        var mockRepo = new Mock<ITagRepository>();
        mockRepo.Setup(repo => repo.GetTagById(1))
            .Returns(GetTestTag());
        var controller = new TagsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetTagById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Tag>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); 
    }

    [Fact]
    public void GetTagById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<ITagRepository>();
        var controller = new TagsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetTagById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetTagById_ReturnsNotFound_WhenTagNotFound()
    {
        // Arrange
        var mockRepo = new Mock<ITagRepository>();
        mockRepo.Setup(repo => repo.GetTagById(1))
            .Returns((Tag)null);
        var controller = new TagsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetTagById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Tag> GetTestTags()
    {
        return new List<Tag>
        {
            new Tag { Id = 1, Name = "Tag1" },
            new Tag { Id = 2, Name = "Tag2" },
        };
    }

    private Tag GetTestTag()
    {
        return new Tag { Id = 1, Name = "Tag1" };
    }
}
