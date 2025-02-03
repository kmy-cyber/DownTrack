using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class EquipmentControllerTests
{
    private readonly IEquipmentQueryServices _fakeQueryService;
    private readonly IEquipmentCommandServices _fakeCommandService;
    private readonly EquipmentController _controller;

    public EquipmentControllerTests()
    {
        _fakeQueryService = A.Fake<IEquipmentQueryServices>();
        _fakeCommandService = A.Fake<IEquipmentCommandServices>();
        _controller = new EquipmentController(_fakeCommandService, _fakeQueryService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateEquipment_ShouldReturnOkResult()
    {
        // Arrange
        var equipmentDto = new EquipmentDto
        {
            Id = 1,
            Name = "Test Equipment"
        };

        A.CallTo(() => _fakeCommandService.CreateAsync(equipmentDto))
            .Returns(Task.FromResult(equipmentDto));

        // Act
        var result = await _controller.CreateEquipment(equipmentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Equipment added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(equipmentDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateEquipment_ShouldReturnOkResult()
    {
        // Arrange
        var equipmentDto = new EquipmentDto
        {
            Id = 1,
            Name = "Updated Equipment"
        };

        A.CallTo(() => _fakeCommandService.UpdateAsync(equipmentDto))
            .Returns(Task.FromResult(equipmentDto));

        // Act
        var result = await _controller.UpdateEquipment(equipmentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(equipmentDto, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(equipmentDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteEquipment_ShouldReturnOkResult()
    {
        // Arrange
        int equipmentId = 1;

        A.CallTo(() => _fakeCommandService.DeleteAsync(equipmentId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteEquipment(equipmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Equipment deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(equipmentId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetUserById_ShouldReturnOkResultWithEquipment()
    {
        // Arrange
        int equipmentId = 1;
        var expectedEquipment = new GetEquipmentDto
        {
            Id = equipmentId,
            Name = "Test Equipment"
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(equipmentId))
            .Returns(Task.FromResult(expectedEquipment));

        // Act
        var result = await _controller.GetUserById(equipmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedEquipment, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(equipmentId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetUserById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int equipmentId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(equipmentId))
            .Returns(Task.FromResult<GetEquipmentDto>(null!));

        // Act
        var result = await _controller.GetUserById(equipmentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Equipment with ID {equipmentId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedEquipment_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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


        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(A<PagedRequestDto>._))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedEquipment(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(A<PagedRequestDto>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetPagedEquipmentsBySectionManagerId_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto();
        int sectionManagerId = 1;
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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

        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsBySectionManagerIdAsync(A<PagedRequestDto>._, sectionManagerId))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedEquipmentsBySectionManagerId(pagedRequest, sectionManagerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsBySectionManagerIdAsync(A<PagedRequestDto>._, sectionManagerId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetPagedEquipmentsBySectionId_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto();
        int sectionId = 1;
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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


        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsBySectionIdAsync(A<PagedRequestDto>._, sectionId))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedEquipmentsBySectionId(pagedRequest, sectionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsBySectionIdAsync(A<PagedRequestDto>._, sectionId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetPagedEquipmentsByDepartmentId_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto();
        int departmentId = 1;
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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


        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsByDepartmentIdAsync(A<PagedRequestDto>._, departmentId))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedEquipmentsByDepartmentId(pagedRequest, departmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsByDepartmentIdAsync(A<PagedRequestDto>._, departmentId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetActiveEquipment_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto();
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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


        A.CallTo(() => _fakeQueryService.GetActiveEquipment(A<PagedRequestDto>._))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetActiveEquipment(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetActiveEquipment(A<PagedRequestDto>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetPagedAllEquipmentsByName_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto();
        string equipmentName = "Test";
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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


        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsByNameAsync(A<PagedRequestDto>._, equipmentName))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedAllEquipmentsByName(pagedRequest, equipmentName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsByNameAsync(A<PagedRequestDto>._, equipmentName))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetPagedAllEquipmentsByNameAndSectionManagerId_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto();
        string equipmentName = "Test";
        int sectionManagerId = 1;
        var expectedResult = new PagedResultDto<GetEquipmentDto>
        {
            Items = new List<GetEquipmentDto>(),
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


        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsByNameAndSectionManagerAsync(
            A<PagedRequestDto>._, equipmentName, sectionManagerId))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedAllEquipmentsByNameAndSectionManagerId(
            pagedRequest, equipmentName, sectionManagerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetPagedEquipmentsByNameAndSectionManagerAsync(
            A<PagedRequestDto>._, equipmentName, sectionManagerId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion
}