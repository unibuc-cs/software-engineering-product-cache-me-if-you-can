using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Developer_Toolbox.Models;
using Xunit;

public class WeeklyChallengeUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var weeklyChallenge = new WeeklyChallenge();

        // Assert
        Assert.NotNull(weeklyChallenge);
        Assert.IsType<WeeklyChallenge>(weeklyChallenge);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 7);

        var weeklyChallenge = new WeeklyChallenge()
        {
            Title = "Sample Weekly Challenge",
            Description = "This is a sample weekly challenge description",
            Difficulty = "Medium",
            RewardPoints = 100,
            StartDate = startDate,
            EndDate = endDate,
            WeeklyChallengeExercises = new List<WeeklyChallengeExercise>()
        };

        // Act & Assert
        Assert.Equal("Sample Weekly Challenge", weeklyChallenge.Title);
        Assert.Equal("This is a sample weekly challenge description", weeklyChallenge.Description);
        Assert.Equal("Medium", weeklyChallenge.Difficulty);
        Assert.Equal(100, weeklyChallenge.RewardPoints);
        Assert.Equal(startDate, weeklyChallenge.StartDate);
        Assert.Equal(endDate, weeklyChallenge.EndDate);
        Assert.NotNull(weeklyChallenge.WeeklyChallengeExercises);
    }

    [Fact]
    public void Collections_InitializedCorrectly()
    {
        // Arrange
        var weeklyChallenge = new WeeklyChallenge()
        {
            WeeklyChallengeExercises = new List<WeeklyChallengeExercise>()
        };

        // Act & Assert
        Assert.Empty(weeklyChallenge.WeeklyChallengeExercises);
    }

    [Fact]
    public void Validate_EndDateBeforeStartDate_ReturnsValidationError()
    {
        // Arrange
        var weeklyChallenge = new WeeklyChallenge()
        {
            Title = "Invalid Challenge",
            Description = "This challenge has an invalid date range",
            Difficulty = "Easy",
            RewardPoints = 50,
            StartDate = new DateTime(2025, 1, 10),
            EndDate = new DateTime(2025, 1, 5),
            WeeklyChallengeExercises = new List<WeeklyChallengeExercise>() { new WeeklyChallengeExercise() }
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(weeklyChallenge);

        // Act
        Validator.TryValidateObject(weeklyChallenge, validationContext, validationResults, true);

        // Assert
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "End date cannot be earlier than start date.");
    }


    [Fact]
    public void Validate_ValidWeeklyChallenge_NoValidationErrors()
    {
        // Arrange
        var weeklyChallenge = new WeeklyChallenge()
        {
            Title = "Valid Challenge",
            Description = "This challenge has valid properties",
            Difficulty = "Hard",
            RewardPoints = 200,
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 1, 10),
            WeeklyChallengeExercises = new List<WeeklyChallengeExercise>() { new WeeklyChallengeExercise() }
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(weeklyChallenge);

        // Act
        Validator.TryValidateObject(weeklyChallenge, validationContext, validationResults, true);

        // Assert
        Assert.Empty(validationResults);
    }
}
