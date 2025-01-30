using System;
using System.ComponentModel.DataAnnotations;
using Developer_Toolbox.Models;
using Xunit;

public class NotificationUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var notification = new Notification();

        // Assert
        Assert.NotNull(notification);
        Assert.IsType<Notification>(notification);
        Assert.False(notification.IsRead); // Verifică valoarea implicită
        Assert.NotNull(notification.CreatedAt);
        Assert.True(notification.CreatedAt <= DateTime.Now);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var createdAt = DateTime.Now;
        var notification = new Notification()
        {
            UserId = "user123",
            Message = "New challenge available!",
            Link = "/challenges/weekly",
            IsRead = true,
            CreatedAt = createdAt
        };

        // Act & Assert
        Assert.Equal("user123", notification.UserId);
        Assert.Equal("New challenge available!", notification.Message);
        Assert.Equal("/challenges/weekly", notification.Link);
        Assert.True(notification.IsRead);
        Assert.Equal(createdAt, notification.CreatedAt);
    }

    [Fact]
    public void Message_RequiredValidation()
    {
        // Verifică că Message are atributul Required
        var requiredAttribute = typeof(Notification)
            .GetProperty("Message")
            .GetCustomAttributes(typeof(RequiredAttribute), false)
            .FirstOrDefault() as RequiredAttribute;

        Assert.NotNull(requiredAttribute);
    }

    [Fact]
    public void MaxLength_Validation()
    {
        // Verifică limitarea MaxLength pentru Message și Link
        var messageMaxLength = typeof(Notification)
            .GetProperty("Message")
            .GetCustomAttributes(typeof(MaxLengthAttribute), false)
            .FirstOrDefault() as MaxLengthAttribute;

        var linkMaxLength = typeof(Notification)
            .GetProperty("Link")
            .GetCustomAttributes(typeof(MaxLengthAttribute), false)
            .FirstOrDefault() as MaxLengthAttribute;

        Assert.NotNull(messageMaxLength);
        Assert.Equal(500, messageMaxLength.Length);
        Assert.NotNull(linkMaxLength);
        Assert.Equal(500, linkMaxLength.Length);
    }

    [Fact]
    public void DefaultValues_Test()
    {
        // Arrange & Act
        var notification = new Notification();

        // Assert
        Assert.False(notification.IsRead);
        Assert.NotNull(notification.CreatedAt);
        Assert.True((DateTime.Now - notification.CreatedAt).TotalMinutes < 1);
    }
}