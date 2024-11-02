using Developer_Toolbox.Models;
using Xunit;

public class TagUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var tag = new Tag();

        // Assert
        Assert.NotNull(tag);
        Assert.IsType<Tag>(tag);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var tag = new Tag
        {
            Name = "SampleTag"
        };

        // Act & Assert
        Assert.Equal("SampleTag", tag.Name);
    }
}
