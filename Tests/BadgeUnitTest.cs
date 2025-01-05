using Developer_Toolbox.Models;
using Xunit;

public class BadgeUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        var badge = new Badge();
        Assert.NotNull(badge);
        Assert.IsType<Badge>(badge);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        var badge = new Badge
        {
            Id = 1,
            Title = "Task Master",
            Description = "Awarded for completing 10 tasks",
            Image = "/img/badges/task_master.png",
            TargetActivityId = 2,
            TargetNoOfTimes = 10
        };

        Assert.Equal(1, badge.Id);
        Assert.Equal("Task Master", badge.Title);
        Assert.Equal("Awarded for completing 10 tasks", badge.Description);
        Assert.Equal("/img/badges/task_master.png", badge.Image);
        Assert.Equal(2, badge.TargetActivityId);
        Assert.Equal(10, badge.TargetNoOfTimes);
    }

    [Fact]
    public void NullProperties_Allowed()
    {
        var badge = new Badge
        {
            Id = 1,
            Title = null,
            Description = null,
            Image = null,
            TargetActivityId = null,
            TargetNoOfTimes = null
        };

        Assert.Equal(1, badge.Id);
        Assert.Null(badge.Title);
        Assert.Null(badge.Description);
        Assert.Null(badge.Image);
        Assert.Null(badge.TargetActivityId);
        Assert.Null(badge.TargetNoOfTimes);
    }
}
