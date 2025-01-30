using System;
using Developer_Toolbox.Models;
using Xunit;

public class LockedSolutionUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var lockedSolution = new LockedSolution();

        // Assert
        Assert.NotNull(lockedSolution);
        Assert.IsType<LockedSolution>(lockedSolution);
        Assert.NotNull(lockedSolution.CreatedAt);
        Assert.True(lockedSolution.CreatedAt <= DateTime.Now);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var createdAt = DateTime.Now;
        var lockedSolution = new LockedSolution()
        {
            SolutionCode = "public class Solution { }",
            Score = 95,
            LockedExerciseId = 1,
            UserId = "user123",
            CreatedAt = createdAt
        };

        // Act & Assert
        Assert.Equal("public class Solution { }", lockedSolution.SolutionCode);
        Assert.Equal(95, lockedSolution.Score);
        Assert.Equal(1, lockedSolution.LockedExerciseId);
        Assert.Equal("user123", lockedSolution.UserId);
        Assert.Equal(createdAt, lockedSolution.CreatedAt);
        Assert.Null(lockedSolution.LockedExercise);
        Assert.Null(lockedSolution.User);
    }

    [Fact]
    public void CreatedAt_DefaultValueTest()
    {
        // Arrange & Act
        var lockedSolution = new LockedSolution();

        // Assert
        Assert.NotNull(lockedSolution.CreatedAt);
        Assert.True((DateTime.Now - lockedSolution.CreatedAt.Value).TotalMinutes < 1);
    }
}