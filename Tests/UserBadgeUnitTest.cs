using Developer_Toolbox.Models;
using System;
using Xunit;

public class UserBadgeUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var userBadge = new UserBadge();

        // Assert
        Assert.NotNull(userBadge);
        Assert.IsType<UserBadge>(userBadge);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var userBadge = new UserBadge()
        {
            UserId = "User123",
            BadgeId = 1,
            ReceivedAt = DateTime.Now
        };

        // Act & Assert
        Assert.Equal("User123", userBadge.UserId);
        Assert.Equal(1, userBadge.BadgeId);
        Assert.NotNull(userBadge.ReceivedAt);
    }

    [Fact]
    public void Properties_SetNullValues()
    {
        // Arrange
        var userBadge = new UserBadge()
        {
            UserId = null,
            BadgeId = null,
            ReceivedAt = null
        };

        // Act & Assert
        Assert.Null(userBadge.UserId);
        Assert.Null(userBadge.BadgeId);
        Assert.Null(userBadge.ReceivedAt);
    }

    [Fact]
    public void Constructor_WithValidValues()
    {
        // Arrange & Act
        var userBadge = new UserBadge
        {
            UserId = "User123",
            BadgeId = 2,
            ReceivedAt = DateTime.Now.AddDays(-1)
        };

        // Assert
        Assert.NotNull(userBadge);
        Assert.Equal("User123", userBadge.UserId);
        Assert.Equal(2, userBadge.BadgeId);
        Assert.NotNull(userBadge.ReceivedAt);
        Assert.True(userBadge.ReceivedAt <= DateTime.Now);
    }
}
