using Developer_Toolbox.Models;
using Xunit;

public class ReactionUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var reaction = new Reaction();

        // Assert
        Assert.NotNull(reaction);
        Assert.IsType<Reaction>(reaction);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var reaction = new Reaction()
        {
            UserId = "user123",
            QuestionId = 1,
            Liked = true,
            Disliked = false
        };

        // Act & Assert
        Assert.Equal("user123", reaction.UserId);
        Assert.Equal(1, reaction.QuestionId);
        Assert.True(reaction.Liked);
        Assert.False(reaction.Disliked);
    }
}
