using Developer_Toolbox.Models;
using Xunit;

public class BookmarkUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var bookmark = new Bookmark();

        // Assert
        Assert.NotNull(bookmark);
        Assert.IsType<Bookmark>(bookmark);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var bookmark = new Bookmark()
        {
            UserId = "user123",
            QuestionId = 1
        };

        // Act & Assert
        Assert.Equal("user123", bookmark.UserId);
        Assert.Equal(1, bookmark.QuestionId);
    }
}
