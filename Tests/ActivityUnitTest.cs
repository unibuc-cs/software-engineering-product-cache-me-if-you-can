using System;
using System.ComponentModel.DataAnnotations;
using Developer_Toolbox.Models;
using Xunit;

public class ActivityUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var activity = new Activity();

        // Assert
        Assert.NotNull(activity);
        Assert.IsType<Activity>(activity);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var activity = new Activity()
        {
            Description = "Complete daily challenge",
            ReputationPoints = 50,
            isPracticeRelated = true,
            RelatedBadges = new List<Badge>()
        };

        // Act & Assert
        Assert.Equal("Complete daily challenge", activity.Description);
        Assert.Equal(50, activity.ReputationPoints);
        Assert.True(activity.isPracticeRelated);
        Assert.NotNull(activity.RelatedBadges);
        Assert.Empty(activity.RelatedBadges);
    }

    [Fact]
    public void ReputationPoints_RangeValidation()
    {
        // Arrange
        var activity = new Activity();

        // Act & Assert
        var validPoints = new int[] { 0, 50, 100 };
        var invalidPoints = new int[] { -1, 101 };

        foreach (var points in validPoints)
        {
            activity.ReputationPoints = points;
            Assert.Equal(points, activity.ReputationPoints);
        }

        // Verificăm că validarea Range se aplică
        var rangeAttribute = typeof(Activity)
            .GetProperty("ReputationPoints")
            .GetCustomAttributes(typeof(RangeAttribute), false)
            .FirstOrDefault() as RangeAttribute;

        Assert.NotNull(rangeAttribute);
        Assert.Equal(0, rangeAttribute.Minimum);
        Assert.Equal(100, rangeAttribute.Maximum);
    }
}