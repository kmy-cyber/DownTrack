using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;

public class DepartmentControllerTests
{
    private readonly IDepartmentQueryServices _fakeQueryService;
    private readonly IDepartmentCommandServices _fakeCommandService;
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _fakeQueryService = A.Fake<IDepartmentQueryServices>();
        _fakeCommandService = A.Fake<IDepartmentCommandServices>();
        _controller = new DepartmentController(_fakeQueryService, _fakeCommandService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateDepartment_ShouldReturnOkResult()
    {
        // Arrange
        var departmentDto = new DepartmentDto();
        var createdDepartment = new DepartmentDto();
        A.CallTo(() => _fakeCommandService.CreateAsync(departmentDto))
            .Returns(Task.FromResult(createdDepartment));

        // Act
        var result = await _controller.CreateDepartmen(departmentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Department added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(departmentDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateDepartment_ShouldReturnOkResult()
    {
        // Arrange
        var departmentDto = new DepartmentDto();
        var updatedDepartment = new DepartmentDto();
        A.CallTo(() => _fakeCommandService.UpdateAsync(departmentDto))
            .Returns(Task.FromResult(updatedDepartment));

        // Act
        var result = await _controller.UpdateDepartment(departmentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedDepartment, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(departmentDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteDepartment_ShouldReturnOkResult()
    {
        // Arrange
        int departmentId = 1;
        A.CallTo(() => _fakeCommandService.DeleteAsync(departmentId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteDepartment(departmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Department deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(departmentId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetDepartmentById_WhenDepartmentExists_ShouldReturnOkResult()
    {
        // Arrange
        int departmentId = 1;
        var expectedDepartment = new GetDepartmentDto();
        A.CallTo(() => _fakeQueryService.GetByIdAsync(departmentId))
            .Returns(Task.FromResult(expectedDepartment));

        // Act
        var result = await _controller.GetDepartmentById(departmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedDepartment, okResult.Value);
    }

    [Fact]
    public async Task GetDepartmentById_WhenDepartmentDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        int departmentId = 1;
        GetDepartmentDto? nullDepartment = null;
        A.CallTo(() => _fakeQueryService.GetByIdAsync(departmentId))
            .Returns(Task.FromResult(nullDepartment));

        // Act
        var result = await _controller.GetDepartmentById(departmentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Department with ID {departmentId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedDepartment_ShouldReturnOkResult()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        
        var expectedResult = new PagedResultDto<GetDepartmentDto>
        {
            Items = new List<GetDepartmentDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1,
            
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/Department/GetPaged");

        A.CallTo(() => httpRequest.Scheme).Returns(httpScheme);
        A.CallTo(() => httpRequest.Host).Returns(httpHost);
        A.CallTo(() => httpRequest.Path).Returns(httpPath);
        A.CallTo(() => httpContext.Request).Returns(httpRequest);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(A<PagedRequestDto>.That.Matches(
            x => x.PageNumber == pagedRequest.PageNumber && 
                 x.PageSize == pagedRequest.PageSize && 
                 x.BaseUrl == $"{httpScheme}://{httpHost}{httpPath}")))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedDepartment(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult()
    {
        // Arrange
        var expectedDepartments = new List<GetDepartmentDto>();
        A.CallTo(() => _fakeQueryService.ListAsync())
            .Returns(Task.FromResult((IEnumerable<GetDepartmentDto>)expectedDepartments));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedDepartments, okResult.Value);
    }

    [Fact]
    public async Task GetAllDepartmentInSection_ShouldReturnOkResult()
    {
        // Arrange
        int sectionId = 1;
        var expectedDepartments = new List<GetDepartmentDto>();
        A.CallTo(() => _fakeQueryService.GetAllDepartmentsInSection(sectionId))
            .Returns(Task.FromResult((IEnumerable<GetDepartmentDto>)expectedDepartments));

        // Act
        var result = await _controller.GetAllDepartmentInSection(sectionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedDepartments, okResult.Value);
    }

    [Fact]
    public async Task GetPagedAllDepartmentInSection_ShouldReturnOkResult()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        int sectionId = 1;
        
        var expectedResult = new PagedResultDto<GetDepartmentDto>
        {
            Items = new List<GetDepartmentDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1,
            
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/Department/Get_Paged_AllDepartment_In_Section");

        A.CallTo(() => httpRequest.Scheme).Returns(httpScheme);
        A.CallTo(() => httpRequest.Host).Returns(httpHost);
        A.CallTo(() => httpRequest.Path).Returns(httpPath);
        A.CallTo(() => httpContext.Request).Returns(httpRequest);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        A.CallTo(() => _fakeQueryService.GetPagedAllDepartmentsInSection(
            A<PagedRequestDto>.That.Matches(
                x => x.PageNumber == pagedRequest.PageNumber && 
                     x.PageSize == pagedRequest.PageSize && 
                     x.BaseUrl == $"{httpScheme}://{httpHost}{httpPath}"),
            sectionId))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedAllDepartmentInSection(pagedRequest, sectionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    #endregion
}