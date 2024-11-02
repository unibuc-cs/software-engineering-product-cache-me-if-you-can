using System;
using Developer_Toolbox.Models;
using Xunit;

public class AnswerUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var answer = new Answer();

        // Assert
        Assert.NotNull(answer);
        Assert.IsType<Answer>(answer);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var answer = new Answer()
        {
            Date = new DateTime(2023, 1, 1),
            LikesNr = 10,
            DislikesNr = 2,
            Content = "Sample content",
            UserId = "user123",
            QuestionId = 1
        };

        // Act & Assert
        Assert.Equal(new DateTime(2023, 1, 1), answer.Date);
        Assert.Equal(10, answer.LikesNr);
        Assert.Equal(2, answer.DislikesNr);
        Assert.Equal("Sample content", answer.Content);
        Assert.Equal("user123", answer.UserId);
        Assert.Equal(1, answer.QuestionId);
    }
}
