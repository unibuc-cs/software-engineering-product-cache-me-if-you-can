using System;
using Developer_Toolbox.Models;
using Xunit;

public class BadgeChallengeUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var badgeId = 1;
        var challengeId = 2;
        var badgeChallenge = new BadgeChallenge(badgeId, challengeId);

        // Assert
        Assert.NotNull(badgeChallenge);
        Assert.IsType<BadgeChallenge>(badgeChallenge);
        Assert.Equal(badgeId, badgeChallenge.BadgeId);
        Assert.Equal(challengeId, badgeChallenge.WeeklyChallengeId);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var badgeChallenge = new BadgeChallenge(1, 2)
        {
            Badge = null,
            WeeklyChallenge = null
        };

        // Act & Assert
        Assert.Equal(1, badgeChallenge.BadgeId);
        Assert.Equal(2, badgeChallenge.WeeklyChallengeId);
        Assert.Null(badgeChallenge.Badge);
        Assert.Null(badgeChallenge.WeeklyChallenge);
    }

    [Fact]
    public void Constructor_AcceptsNullValues()
    {
        // Arrange & Act
        var badgeChallenge = new BadgeChallenge(null, null);

        // Assert
        Assert.Null(badgeChallenge.BadgeId);
        Assert.Null(badgeChallenge.WeeklyChallengeId);
        Assert.Null(badgeChallenge.Badge);
        Assert.Null(badgeChallenge.WeeklyChallenge);
    }
}