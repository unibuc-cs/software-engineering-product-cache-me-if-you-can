using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class BookmarksControllerTests
{
    [Fact]
    public void GetAllBookmarks_ReturnsViewResult_WithListOfBookmarks()
    {
        // Arrange
        var mockRepo = new Mock<IBookmarkRepository>();
        mockRepo.Setup(repo => repo.GetAllBookmarks())
            .Returns(GetTestBookmarks());
        var controller = new BookmarksController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllBookmarks();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Bookmark>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 bookmarks în model
    }

    [Fact]
    public void GetBookmarkById_ReturnsViewResult_WithBookmark()
    {
        // Arrange
        var mockRepo = new Mock<IBookmarkRepository>();
        mockRepo.Setup(repo => repo.GetBookmarkById(1))
            .Returns(GetTestBookmark());
        var controller = new BookmarksController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetBookmarkById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Bookmark>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetBookmarkById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<IBookmarkRepository>();
        var controller = new BookmarksController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetBookmarkById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetBookmarkById_ReturnsNotFound_WhenBookmarkNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IBookmarkRepository>();
        mockRepo.Setup(repo => repo.GetBookmarkById(1))
            .Returns((Bookmark)null);
        var controller = new BookmarksController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetBookmarkById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Bookmark> GetTestBookmarks()
    {
        return new List<Bookmark>
        {
            new Bookmark { Id = 1, UserId = "1", QuestionId = 1 },
            new Bookmark { Id = 2, UserId = "2", QuestionId = 2 },
        };
    }

    private Bookmark GetTestBookmark()
    {
        return new Bookmark { Id = 1, UserId = "1", QuestionId = 1 };
    }
}
