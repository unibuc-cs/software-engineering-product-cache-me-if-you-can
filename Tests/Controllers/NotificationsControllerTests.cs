using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

public class NotificationsControllerTests
{
    [Fact]
    public void GetAllNotifications_ReturnsViewResult_WithListOfNotifications()
    {
        // Arrange
        var mockRepo = new Mock<INotificationRepository>();
        mockRepo.Setup(repo => repo.GetAllNotifications())
            .Returns(GetTestNotifications());
        var controller = new NotificationsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllNotifications();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Notification>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // Check if there are 2 notifications in the model
    }

    [Fact]
    public void GetNotificationById_ReturnsViewResult_WithNotification()
    {
        // Arrange
        var mockRepo = new Mock<INotificationRepository>();
        mockRepo.Setup(repo => repo.GetNotificationById(1))
            .Returns(GetTestNotification());
        var controller = new NotificationsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetNotificationById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Notification>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // Check if the ID is correct
    }

    [Fact]
    public void GetNotificationById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<INotificationRepository>();
        var controller = new NotificationsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetNotificationById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetNotificationById_ReturnsNotFound_WhenNotificationNotFound()
    {
        // Arrange
        var mockRepo = new Mock<INotificationRepository>();
        mockRepo.Setup(repo => repo.GetNotificationById(1))
            .Returns((Notification)null);
        var controller = new NotificationsController(null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetNotificationById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Notification> GetTestNotifications()
    {
        return new List<Notification>
        {
            new Notification
            {
                Id = 1,
                Message = "Test Notification 1",
                Link = "http://example.com",
                IsRead = false,
                CreatedAt = DateTime.Now
            },
            new Notification
            {
                Id = 2,
                Message = "Test Notification 2",
                Link = "http://example2.com",
                IsRead = true,
                CreatedAt = DateTime.Now.AddMinutes(-10)
            }
        };
    }

    private Notification GetTestNotification()
    {
        return new Notification
        {
            Id = 1,
            Message = "Test Notification 1",
            Link = "http://example.com",
            IsRead = false,
            CreatedAt = DateTime.Now
        };
    }
}
