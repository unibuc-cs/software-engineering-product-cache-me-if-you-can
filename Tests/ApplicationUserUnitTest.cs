using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ApplicationUserUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var user = new ApplicationUser();

        // Assert
        Assert.NotNull(user);
        Assert.IsType<ApplicationUser>(user);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var user = new ApplicationUser()
        {
            FirstName = "John",
            LastName = "Doe",
            ReputationPoints = 100,
            EmailAddress = "john.doe@example.com",
            Birthday = new DateTime(1990, 1, 1),
            Description = "Sample description"
        };

        // Act & Assert
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal(100, user.ReputationPoints);
        Assert.Equal("john.doe@example.com", user.EmailAddress);
        Assert.Equal(new DateTime(1990, 1, 1), user.Birthday);
        Assert.Equal("Sample description", user.Description);
    }

    [Fact]
    public void Collections_InitializedCorrectly()
    {
        // Arrange
        var user = new ApplicationUser()
        {
            Exercises = new List<Exercise>(),
            Solutions = new List<Solution>(),
            Questions = new List<Question>(),
            Answers = new List<Answer>(),
            Bookmarks = new List<Bookmark>(),
            AllRoles = new List<SelectListItem>()
        };

        // Act & Assert
        Assert.Empty(user.Exercises);
        Assert.Empty(user.Solutions);
        Assert.Empty(user.Questions);
        Assert.Empty(user.Answers);
        Assert.Empty(user.Bookmarks);
        Assert.Empty(user.AllRoles);
    }
}
