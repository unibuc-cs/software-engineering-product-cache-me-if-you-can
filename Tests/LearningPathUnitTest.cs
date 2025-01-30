using System;
using Developer_Toolbox.Models;
using Xunit;

public class LearningPathUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var learningPath = new LearningPath();

        // Assert
        Assert.NotNull(learningPath);
        Assert.IsType<LearningPath>(learningPath);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var learningPath = new LearningPath()
        {
            Name = "Beginner Programming",
            UserId = "user123",
            Description = "A comprehensive path for beginners",
            LockedExercises = new List<LockedExercise>()
        };

        // Act & Assert
        Assert.Equal("Beginner Programming", learningPath.Name);
        Assert.Equal("user123", learningPath.UserId);
        Assert.Equal("A comprehensive path for beginners", learningPath.Description);
        Assert.NotNull(learningPath.LockedExercises);
        Assert.Empty(learningPath.LockedExercises);
    }
}