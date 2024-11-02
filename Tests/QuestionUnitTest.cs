using System;
using System.Collections.Generic;
using Developer_Toolbox.Models;
using Xunit;

public class QuestionUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var question = new Question();

        // Assert
        Assert.NotNull(question);
        Assert.IsType<Question>(question);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var question = new Question()
        {
            Title = "Sample Question",
            Description = "This is a sample question description",
            CreatedDate = new DateTime(2023, 1, 1),
            LikesNr = 10,
            DislikesNr = 2
        };

        // Act & Assert
        Assert.Equal("Sample Question", question.Title);
        Assert.Equal("This is a sample question description", question.Description);
        Assert.Equal(new DateTime(2023, 1, 1), question.CreatedDate);
        Assert.Equal(10, question.LikesNr);
        Assert.Equal(2, question.DislikesNr);
    }

    [Fact]
    public void Collections_InitializedCorrectly()
    {
        // Arrange
        var question = new Question()
        {
            QuestionTags = new List<QuestionTag>(),
            Answers = new List<Answer>(),
            Bookmarks = new List<Bookmark>(),
            Reactions = new List<Reaction>()
        };

        // Act & Assert
        Assert.Empty(question.QuestionTags);
        Assert.Empty(question.Answers);
        Assert.Empty(question.Bookmarks);
        Assert.Empty(question.Reactions);
    }
}
