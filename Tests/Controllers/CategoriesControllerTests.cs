using Developer_Toolbox.Controllers;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class CategoriesControllerTests
{
    [Fact]
    public void GetAllCategories_ReturnsViewResult_WithListOfCategories()
    {
        // Arrange
        var mockRepo = new Mock<ICategoryRepository>();
        mockRepo.Setup(repo => repo.GetAllCategories())
            .Returns(GetTestCategories());
        var controller = new CategoriesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetAllCategories();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count()); // verificăm dacă avem 2 categorii în model
    }

    [Fact]
    public void GetCategoryById_ReturnsViewResult_WithCategory()
    {
        // Arrange
        var mockRepo = new Mock<ICategoryRepository>();
        mockRepo.Setup(repo => repo.GetCategoryById(1))
            .Returns(GetTestCategory());
        var controller = new CategoriesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetCategoryById(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id); // verificăm dacă ID-ul este corect
    }

    [Fact]
    public void GetCategoryById_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockRepo = new Mock<ICategoryRepository>();
        var controller = new CategoriesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetCategoryById(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetCategoryById_ReturnsNotFound_WhenCategoryNotFound()
    {
        // Arrange
        var mockRepo = new Mock<ICategoryRepository>();
        mockRepo.Setup(repo => repo.GetCategoryById(1))
            .Returns((Category)null);
        var controller = new CategoriesController(null, null, null, null, mockRepo.Object);

        // Act
        var result = controller.GetCategoryById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<Category> GetTestCategories()
    {
        return new List<Category>
    {
        new Category { Id = 1, CategoryName = "Category 1", Logo = "/images/categories/cat1.png" },
        new Category { Id = 2, CategoryName = "Category 2", Logo = "/images/categories/cat2.png" }
    };
    }

    private Category GetTestCategory()
    {
        return new Category { Id = 1, CategoryName = "Category 1", Logo = "/images/categories/cat1.png" };
    }

}
