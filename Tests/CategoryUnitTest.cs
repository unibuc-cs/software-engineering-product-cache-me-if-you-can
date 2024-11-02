using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Developer_Toolbox.Models;
using Xunit;

public class CategoryUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        var category = new Category();

        Assert.NotNull(category);
        Assert.IsType<Category>(category);
    }

    [Fact]
    public void Create_Test()
    {
        var category = new Category()
        {
            CategoryName = "Test",
            Logo = "test"

        };
        Assert.Equal("Test", category.CategoryName);
        Assert.Equal("test", category.Logo);
    }
}
