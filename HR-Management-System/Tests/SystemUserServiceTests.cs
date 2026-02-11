using HR_Management_System.Models;
using HR_Management_System.Repositories;
using HR_Management_System.Services;
using Xunit;
using Moq;

namespace HR_Management_System.Tests;

public class SystemUserServiceTests
{
    [Fact]
    public async Task CreateAsync_LoginIsWhitespace_ThrowsArgumentException()
    {
        // Arrange
        var repoMock = new Mock<ISystemUserRepository>();
        repoMock.Setup(r => r.LoginExists(It.IsAny<string>())).ReturnsAsync(false);

        var service = new SystemUserService(repoMock.Object);

        var user = new Systemuser
        {
            Login = "   ", // whitespace -> invalid
            Password = "pwd123"
        };

        // Act & Assert: ArgumentException expected, paramName == "login"
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(user));
        Assert.Equal("login", ex.ParamName);
    }

    [Fact]
    public async Task CreateAsync_LoginAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var repoMock = new Mock<ISystemUserRepository>();
        repoMock.Setup(r => r.LoginExists("existing")).ReturnsAsync(true);

        var service = new SystemUserService(repoMock.Object);

        var user = new Systemuser
        {
            Login = "existing",
            Password = "pwd123"
        };

        // Act & Assert: InvalidOperationException expected with message about uniqueness
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(user));
        Assert.Contains("already in use", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}