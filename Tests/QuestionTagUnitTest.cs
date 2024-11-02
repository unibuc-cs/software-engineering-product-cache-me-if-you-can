using Developer_Toolbox.Models;
using Xunit;

public class QuestionTagUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var questionTag = new QuestionTag();

        // Assert
        Assert.NotNull(questionTag);
        Assert.IsType<QuestionTag>(questionTag);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var questionTag = new QuestionTag()
        {
            QuestionId = 1,
            TagId = 2
        };

        // Act & Assert
        Assert.Equal(1, questionTag.QuestionId);
        Assert.Equal(2, questionTag.TagId);
    }
}
