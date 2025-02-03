using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class AuthenticationControllerTests
{
    private readonly IIdentityService _fakeIdentityService;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _fakeIdentityService = A.Fake<IIdentityService>();
        _controller = new AuthenticationController(_fakeIdentityService);
    }

    #region Register Tests

    [Fact]
    public async Task RegisterUser_ShouldReturnOkResultWithToken()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            UserName = "testuser",
            Email = "test@test.com",
            Password = "Password123!",
            DepartmentId = 1,
            UserRole = "Administrator"
        };
        var expectedToken = "generated_jwt_token";

        A.CallTo(() => _fakeIdentityService.RegisterUserAsync(registerDto))
            .Returns(Task.FromResult(expectedToken));

        // Act
        var result = await _controller.RegisterUser(registerDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedToken, okResult.Value);
        A.CallTo(() => _fakeIdentityService.RegisterUserAsync(registerDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RegisterUser_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var registerDto = new RegisterUserDto();
        var expectedException = new InvalidOperationException("Registration failed");

        A.CallTo(() => _fakeIdentityService.RegisterUserAsync(registerDto))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.RegisterUser(registerDto));
    }

    #endregion

    #region Login Tests

    [Fact]
    public async Task LoginUser_ShouldReturnOkResultWithToken()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            UserName = "testuser",
            Password = "Password123!"
        };
        var expectedToken = "generated_jwt_token";

        A.CallTo(() => _fakeIdentityService.LoginUserAsync(loginDto))
            .Returns(Task.FromResult(expectedToken));

        // Act
        var result = await _controller.LoginUser(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedToken, okResult.Value);
        A.CallTo(() => _fakeIdentityService.LoginUserAsync(loginDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task LoginUser_WhenServiceReturnsNull_ShouldReturnOkWithNull()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            UserName = "testuser",
            Password = "WrongPassword"
        };

        A.CallTo(() => _fakeIdentityService.LoginUserAsync(loginDto))
            .Returns(Task.FromResult<string>(null!));

        // Act
        var result = await _controller.LoginUser(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
    }

    [Fact]
    public async Task LoginUser_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var loginDto = new LoginUserDto();
        var expectedException = new InvalidOperationException("Login failed");

        A.CallTo(() => _fakeIdentityService.LoginUserAsync(loginDto))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.LoginUser(loginDto));
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task UpdateUser_ShouldReturnOkResult()
    {
        // Arrange
        var updateDto = new UpdateUserDto
        {
            Id = 123,
            Name = "newusername",
            Email = "newemail@test.com",
            Password = "NewPassword123!"
        };

        A.CallTo(() => _fakeIdentityService.UpdateUserAsync(updateDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateUser(updateDto);

        // Assert
        Assert.IsType<OkResult>(result);
        A.CallTo(() => _fakeIdentityService.UpdateUserAsync(updateDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateUser_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var updateDto = new UpdateUserDto();
        var expectedException = new InvalidOperationException("Update failed");

        A.CallTo(() => _fakeIdentityService.UpdateUserAsync(updateDto))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.UpdateUser(updateDto));
    }

    #endregion
}


