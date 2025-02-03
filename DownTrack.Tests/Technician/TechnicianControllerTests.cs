using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class TechnicianControllerTests
{
    private readonly ITechnicianQueryServices _fakeQueryService;
    private readonly ITechnicianCommandServices _fakeCommandService;
    private readonly TechnicianController _controller;

    public TechnicianControllerTests()
    {
        _fakeQueryService = A.Fake<ITechnicianQueryServices>();
        _fakeCommandService = A.Fake<ITechnicianCommandServices>();
        _controller = new TechnicianController(_fakeQueryService, _fakeCommandService);
    }

    #region Query Tests

    [Fact]
    public async Task GetTechnicianById_ShouldReturnOkResultWithTechnician()
    {
        // Arrange
        int technicianId = 1;
        var expectedTechnician = new GetTechnicianDto
        {
            Id = technicianId,
           
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(technicianId))
            .Returns(Task.FromResult(expectedTechnician));

        // Act
        var result = await _controller.GetTechnicianById(technicianId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedTechnician, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(technicianId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetTechnicianById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int technicianId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(technicianId))
            .Returns(Task.FromResult<GetTechnicianDto>(null!));

        // Act
        var result = await _controller.GetTechnicianById(technicianId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Technician with ID {technicianId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedTechnician_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        var expectedResult = new PagedResultDto<GetTechnicianDto>
        {
            Items = new List<GetTechnicianDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/Technician/GetPaged");

        A.CallTo(() => httpRequest.Scheme).Returns(httpScheme);
        A.CallTo(() => httpRequest.Host).Returns(httpHost);
        A.CallTo(() => httpRequest.Path).Returns(httpPath);
        A.CallTo(() => httpContext.Request).Returns(httpRequest);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedTechnician(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .MustHaveHappenedOnceExactly();
    }

    #endregion
}