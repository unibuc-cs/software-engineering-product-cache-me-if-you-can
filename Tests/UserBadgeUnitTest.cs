using System;
using Developer_Toolbox.Models;
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
        var receivedDate = new DateTime(2023, 1, 1);
        var userBadge = new UserBadge()
        {
            UserId = "user123",
            BadgeId = 1,
            ReceivedAt = receivedDate
        };

        // Act & Assert
        Assert.Equal("user123", userBadge.UserId);
        Assert.Equal(1, userBadge.BadgeId);
        Assert.Equal(receivedDate, userBadge.ReceivedAt);
        Assert.Null(userBadge.User);
        Assert.Null(userBadge.Badge);
    }
}