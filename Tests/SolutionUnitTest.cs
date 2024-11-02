using Developer_Toolbox.Models;
using Xunit;

public class SolutionUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var solution = new Solution();

        // Assert
        Assert.NotNull(solution);
        Assert.IsType<Solution>(solution);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var solution = new Solution
        {
            SolutionCode = "SampleCode",
            Score = 10,
            ExerciseId = 1,
            UserId = "123"
        };

        // Act & Assert
        Assert.Equal("SampleCode", solution.SolutionCode);
        Assert.Equal(10, solution.Score);
        Assert.Equal(1, solution.ExerciseId);
        Assert.Equal("123", solution.UserId);
    }
}
