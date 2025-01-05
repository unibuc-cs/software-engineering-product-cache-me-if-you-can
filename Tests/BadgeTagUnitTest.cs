using Developer_Toolbox.Models;
using Xunit;

public class BadgeTagUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var badgeTag = new BadgeTag(null, null);

        // Assert
        Assert.NotNull(badgeTag);
        Assert.IsType<BadgeTag>(badgeTag);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var badgeTag = new BadgeTag(1, 2)
        {
            BadgeId = 1,
            TagId = 2
        };

        // Act & Assert
        Assert.Equal(1, badgeTag.BadgeId);
        Assert.Equal(2, badgeTag.TagId);
    }

    [Fact]
    public void Properties_SetNullValues()
    {
        // Arrange
        var badgeTag = new BadgeTag(null, null);

        // Act & Assert
        Assert.Null(badgeTag.BadgeId);
        Assert.Null(badgeTag.TagId);
    }

    [Fact]
    public void Constructor_WithValidValues()
    {
        // Arrange & Act
        var badgeTag = new BadgeTag(1, 2);

        // Assert
        Assert.NotNull(badgeTag);
        Assert.Equal(1, badgeTag.BadgeId);
        Assert.Equal(2, badgeTag.TagId);
    }
}
