using Developer_Toolbox.Models;
using Xunit;

public class ErrorViewModelUnitTest
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange & Act
        var errorViewModel = new ErrorViewModel();

        // Assert
        Assert.NotNull(errorViewModel);
        Assert.IsType<ErrorViewModel>(errorViewModel);
    }

    [Fact]
    public void Properties_InitializedCorrectly()
    {
        // Arrange
        var errorViewModel = new ErrorViewModel
        {
            RequestId = "123"
        };

        // Act & Assert
        Assert.Equal("123", errorViewModel.RequestId);
        Assert.True(errorViewModel.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_ReturnsFalse_WhenRequestIdIsNullOrEmpty()
    {
        // Arrange
        var errorViewModel = new ErrorViewModel
        {
            RequestId = null
        };

        // Act & Assert
        Assert.False(errorViewModel.ShowRequestId);

        // Arrange
        errorViewModel.RequestId = "";

        // Act & Assert
        Assert.False(errorViewModel.ShowRequestId);
    }
}
