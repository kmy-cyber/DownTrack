using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class SectionControllerTests
{
    private readonly ISectionQueryServices _fakeQueryService;
    private readonly ISectionCommandServices _fakeCommandService;
    private readonly SectionController _controller;

    public SectionControllerTests()
    {
        _fakeQueryService = A.Fake<ISectionQueryServices>();
        _fakeCommandService = A.Fake<ISectionCommandServices>();
        _controller = new SectionController(_fakeQueryService, _fakeCommandService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateSection_ShouldReturnOkResult()
    {
        // Arrange
        var sectionDto = new SectionDto();
        var createdSection = new SectionDto(); // This will be returned by the service
        A.CallTo(() => _fakeCommandService.CreateAsync(sectionDto))
            .Returns(Task.FromResult(createdSection));

        // Act
        var result = await _controller.CreateSection(sectionDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Section added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(sectionDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateSection_ShouldReturnOkResult()
    {
        // Arrange
        var sectionDto = new SectionDto();
        var updatedSection = new SectionDto(); // This will be returned by the service
        A.CallTo(() => _fakeCommandService.UpdateAsync(sectionDto))
            .Returns(Task.FromResult(updatedSection));

        // Act
        var result = await _controller.UpdateSection(sectionDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedSection, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(sectionDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteSection_ShouldReturnOkResult()
    {
        // Arrange
        int sectionId = 1;
        A.CallTo(() => _fakeCommandService.DeleteAsync(sectionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteSection(sectionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Section deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(sectionId)).MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetUserById_WhenSectionExists_ShouldReturnOkResult()
    {
        // Arrange
        int sectionId = 1;
        var expectedSection = new GetSectionDto();
        A.CallTo(() => _fakeQueryService.GetByIdAsync(sectionId)).Returns(expectedSection);

        // Act
        var result = await _controller.GetUserById(sectionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedSection, okResult.Value);
    }

    [Fact]
    public async Task GetUserById_WhenSectionDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        int sectionId = 1;
        GetSectionDto? nullSection = null;
        A.CallTo(() => _fakeQueryService.GetByIdAsync(sectionId))
            .Returns(Task.FromResult(nullSection));

        // Act
        var result = await _controller.GetUserById(sectionId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Section with ID {sectionId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetSectionByName_WhenSectionExists_ShouldReturnOkResult()
    {
        // Arrange
        string sectionName = "TestSection";
        var expectedSection = new GetSectionDto();
        A.CallTo(() => _fakeQueryService.GetSectionByNameAsync(sectionName))
            .Returns(Task.FromResult(expectedSection));

        // Act
        var result = await _controller.GetSectionByName(sectionName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedSection, okResult.Value);
    }

    [Fact]
    public async Task GetSectionByName_WhenSectionDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        string sectionName = "TestSection";
        GetSectionDto? nullSection = null;
        A.CallTo(() => _fakeQueryService.GetSectionByNameAsync(sectionName)).Returns(Task.FromResult(nullSection));

        // Act
        var result = await _controller.GetSectionByName(sectionName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Employee with ID {sectionName} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult()
    {
        // Arrange
        var expectedSections = new List<GetSectionDto>();
        A.CallTo(() => _fakeQueryService.ListAsync()).Returns(expectedSections);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedSections, okResult.Value);
    }

[Fact]
public async Task GetPagedSection_ShouldReturnOkResult()
{
    // Arrange
    var pagedRequest = new PagedRequestDto
    {
        PageNumber = 1,
        PageSize = 10
        // BaseUrl will be set by controller
    };
    
    var expectedResult = new PagedResultDto<GetSectionDto>
    {
        Items = new List<GetSectionDto>(),
        TotalCount = 0,
        PageSize = 10,
        PageNumber = 1,
       
    };

    // Mock the HttpContext to support Request.Scheme, Host, and Path
    var httpContext = A.Fake<HttpContext>();
    var httpRequest = A.Fake<HttpRequest>();
    var httpScheme = "https";
    var httpHost = new HostString("localhost:5001");
    var httpPath = new PathString("/api/Section/GetPaged");

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
    var result = await _controller.GetPagedSection(pagedRequest);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(expectedResult, okResult.Value);
}

[Fact]
public async Task GetSectionsByManager_ShouldReturnOkResult()
{
    // Arrange
    var pagedRequest = new PagedRequestDto
    {
        PageNumber = 1,
        PageSize = 10
        // BaseUrl will be set by controller
    };
    int sectionManagerId = 1;
    
    var expectedResult = new PagedResultDto<GetSectionDto>
    {
        Items = new List<GetSectionDto>(),
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

    A.CallTo(() => _fakeQueryService.GetSectionsByManagerAsync(
        A<PagedRequestDto>.That.Matches(
            x => x.PageNumber == pagedRequest.PageNumber && 
                 x.PageSize == pagedRequest.PageSize && 
                 x.BaseUrl == $"{httpScheme}://{httpHost}{httpPath}"),
        sectionManagerId))
        .Returns(Task.FromResult(expectedResult));

    // Act
    var result = await _controller.GetSectionsByManager(pagedRequest, sectionManagerId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(expectedResult, okResult.Value);
}
#endregion
}



















































