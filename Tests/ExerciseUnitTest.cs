using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Xunit;

public class ExerciseUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var exercise = new Exercise();

        // Assert
        Assert.NotNull(exercise);
        Assert.IsType<Exercise>(exercise);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var exercise = new Exercise
        {
            CategoryId = 1,
            Title = "Sample Title",
            Description = "Sample Description",
            Summary = "Sample Summary",
            Restrictions = "Sample Restrictions",
            Examples = "Sample Examples",
            Difficulty = "Easy",
            Date = new DateTime(2023, 1, 1)
        };

        // Act & Assert
        Assert.Equal(1, exercise.CategoryId);
        Assert.Equal("Sample Title", exercise.Title);
        Assert.Equal("Sample Description", exercise.Description);
        Assert.Equal("Sample Summary", exercise.Summary);
        Assert.Equal("Sample Restrictions", exercise.Restrictions);
        Assert.Equal("Sample Examples", exercise.Examples);
        Assert.Equal("Easy", exercise.Difficulty);
        Assert.Equal(new DateTime(2023, 1, 1), exercise.Date);
    }

    [Fact]
    public void Collections_InitializedCorrectly()
    {
        // Arrange
        var exercise = new Exercise
        {
            Solutions = new List<Solution>(),
            Categories = new List<SelectListItem>()
        };

        // Act & Assert
        Assert.Empty(exercise.Solutions);
        Assert.Empty(exercise.Categories);
    }
}
