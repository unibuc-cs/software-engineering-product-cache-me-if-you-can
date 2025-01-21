using System;
using Developer_Toolbox.Models;
using Xunit;

public class WeeklyChallengeExerciseUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var weeklyChallengeExercise = new WeeklyChallengeExercise();

        // Assert
        Assert.NotNull(weeklyChallengeExercise);
        Assert.IsType<WeeklyChallengeExercise>(weeklyChallengeExercise);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var weeklyChallenge = new WeeklyChallenge { Id = 1, Title = "Sample Challenge" };
        var exercise = new Exercise
        {
            Id = 1,
            CategoryId = 10,
            Title = "Sample Exercise",
            Description = "This is a sample exercise description",
            Summary = "Sample summary",
            Restrictions = "None",
            Examples = "Example 1",
            Difficulty = "Easy",
            TestCases = "Test case 1",
            Date = new DateTime(2025, 1, 1)
        };

        var weeklyChallengeExercise = new WeeklyChallengeExercise
        {
            Id = 1,
            WeeklyChallengeId = weeklyChallenge.Id,
            WeeklyChallenge = weeklyChallenge,
            ExerciseId = exercise.Id,
            Exercise = exercise
        };

        // Act & Assert
        Assert.Equal(1, weeklyChallengeExercise.Id);
        Assert.Equal(1, weeklyChallengeExercise.WeeklyChallengeId);
        Assert.Equal(weeklyChallenge, weeklyChallengeExercise.WeeklyChallenge);
        Assert.Equal(1, weeklyChallengeExercise.ExerciseId);
        Assert.Equal(exercise, weeklyChallengeExercise.Exercise);
        Assert.Equal("Sample Exercise", weeklyChallengeExercise.Exercise.Title);
        Assert.Equal("This is a sample exercise description", weeklyChallengeExercise.Exercise.Description);
        Assert.Equal("Sample summary", weeklyChallengeExercise.Exercise.Summary);
        Assert.Equal("None", weeklyChallengeExercise.Exercise.Restrictions);
        Assert.Equal("Example 1", weeklyChallengeExercise.Exercise.Examples);
        Assert.Equal("Easy", weeklyChallengeExercise.Exercise.Difficulty);
        Assert.Equal("Test case 1", weeklyChallengeExercise.Exercise.TestCases);
        Assert.Equal(new DateTime(2025, 1, 1), weeklyChallengeExercise.Exercise.Date);
    }

    [Fact]
    public void NullProperties_Allowed()
    {
        // Arrange
        var weeklyChallengeExercise = new WeeklyChallengeExercise
        {
            Id = 1,
            WeeklyChallengeId = null,
            WeeklyChallenge = null,
            ExerciseId = null,
            Exercise = null
        };

        // Act & Assert
        Assert.Equal(1, weeklyChallengeExercise.Id);
        Assert.Null(weeklyChallengeExercise.WeeklyChallengeId);
        Assert.Null(weeklyChallengeExercise.WeeklyChallenge);
        Assert.Null(weeklyChallengeExercise.ExerciseId);
        Assert.Null(weeklyChallengeExercise.Exercise);
    }
}
