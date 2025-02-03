using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Roles;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;

public class EmployeeControllerTests
{
    private readonly IEmployeeQueryServices _fakeQueryService;
    private readonly IEmployeeCommandServices _fakeCommandService;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _fakeQueryService = A.Fake<IEmployeeQueryServices>();
        _fakeCommandService = A.Fake<IEmployeeCommandServices>();
        _controller = new EmployeeController(_fakeQueryService, _fakeCommandService);
    }

    #region Query Tests

    [Fact]
    public async Task GetEmployeeById_ShouldReturnOkResultWithEmployee()
    {
        // Arrange
        int employeeId = 1;
        var expectedEmployee = new GetEmployeeDto
        {
            Id = employeeId,
            Name = "Test Employee"
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(employeeId))
            .Returns(Task.FromResult(expectedEmployee));

        // Act
        var result = await _controller.GetEmployeeById(employeeId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedEmployee, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(employeeId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetEmployeeById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int employeeId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(employeeId))
            .Returns(Task.FromResult<GetEmployeeDto>(null!));

        // Act
        var result = await _controller.GetEmployeeById(employeeId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Employee with ID {employeeId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetEmployeeByUserName_ShouldReturnOkResultWithEmployee()
    {
        // Arrange
        string username = "testuser";
        var expectedEmployee = new GetEmployeeDto
        {
            Id = 1,
            Name = "Test Employee",
            UserName = username
        };

        A.CallTo(() => _fakeQueryService.GetByUserNameAsync(username))
            .Returns(Task.FromResult(expectedEmployee));

        // Act
        var result = await _controller.GetEmployeeByUserName(username);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedEmployee, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByUserNameAsync(username))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetEmployeeByUserName_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        string username = "nonexistent";

        A.CallTo(() => _fakeQueryService.GetByUserNameAsync(username))
            .Returns(Task.FromResult<GetEmployeeDto>(null!));

        // Act
        var result = await _controller.GetEmployeeByUserName(username);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Employee with ID {username} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllEmployee_ShouldReturnOkResultWithEmployees()
    {
        // Arrange
        var expectedEmployees = new List<GetEmployeeDto>
        {
            new GetEmployeeDto { Id = 1, Name = "Employee 1" },
            new GetEmployeeDto { Id = 2, Name = "Employee 2" }
        };

        A.CallTo(() => _fakeQueryService.ListAsync())
            .Returns(Task.FromResult<IEnumerable<GetEmployeeDto>>(expectedEmployees));

        // Act
        var result = await _controller.GetAllEmployee();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedEmployees, okResult.Value);
        A.CallTo(() => _fakeQueryService.ListAsync())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetPagedEmployee_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
    var expectedResult = new PagedResultDto<GetEmployeeDto>
    {
        Items = new List<GetEmployeeDto>(),
        TotalCount = 0,
        PageSize = 10,
        PageNumber = 1,
      
    };

    // Mock the HttpContext to support Request.Scheme, Host, and Path
    var httpContext = A.Fake<HttpContext>();
    var httpRequest = A.Fake<HttpRequest>();
    var httpScheme = "https";
    var httpHost = new HostString("localhost:5001");
    var httpPath = new PathString("/api/Section/GetSectionsByManager");

    A.CallTo(() => httpRequest.Scheme).Returns(httpScheme);
    A.CallTo(() => httpRequest.Host).Returns(httpHost);
    A.CallTo(() => httpRequest.Path).Returns(httpPath);
    A.CallTo(() => httpContext.Request).Returns(httpRequest);

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = httpContext
    };

        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(A<PagedRequestDto>.That.Matches(
            p => p.PageNumber == pagedRequest.PageNumber && p.PageSize == pagedRequest.PageSize)))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedEmployee(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(A<PagedRequestDto>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllSectionManager_ShouldReturnOkResultWithManagers()
    {
        // Arrange
        var expectedManagers = new List<GetEmployeeDto>
        {
            new GetEmployeeDto { Id = 1, Name = "Manager 1" },
            new GetEmployeeDto { Id = 2, Name = "Manager 2" }
        };

        A.CallTo(() => _fakeQueryService.ListAllByRole(UserRole.SectionManager))
            .Returns(Task.FromResult<IEnumerable<GetEmployeeDto>>(expectedManagers));

        // Act
        var result = await _controller.GetAllSectionManager();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedManagers, okResult.Value);
        A.CallTo(() => _fakeQueryService.ListAllByRole(UserRole.SectionManager))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllShippingSupervisor_ShouldReturnOkResultWithSupervisors()
    {
        // Arrange
        var expectedSupervisors = new List<GetEmployeeDto>
        {
            new GetEmployeeDto { Id = 1, Name = "Supervisor 1" },
            new GetEmployeeDto { Id = 2, Name = "Supervisor 2" }
        };

        A.CallTo(() => _fakeQueryService.ListAllByRole(UserRole.ShippingSupervisor))
            .Returns(Task.FromResult<IEnumerable<GetEmployeeDto>>(expectedSupervisors));

        // Act
        var result = await _controller.GetAllShippingSupervisor();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedSupervisors, okResult.Value);
        A.CallTo(() => _fakeQueryService.ListAllByRole(UserRole.ShippingSupervisor))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Command Tests

    [Fact]
    public async Task DeleteEmployee_ShouldReturnOkResult()
    {
        // Arrange
        int employeeId = 1;

        A.CallTo(() => _fakeCommandService.DeleteAsync(employeeId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteEmployee(employeeId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Employee deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(employeeId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteEmployee_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        int employeeId = 1;
        var expectedException = new InvalidOperationException("Delete failed");

        A.CallTo(() => _fakeCommandService.DeleteAsync(employeeId))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.DeleteEmployee(employeeId));
    }

    #endregion
}