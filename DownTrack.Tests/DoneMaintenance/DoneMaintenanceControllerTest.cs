using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class DoneMaintenanceControllerTests
{
    private readonly IDoneMaintenanceQueryServices _fakeQueryService;
    private readonly IDoneMaintenanceCommandServices _fakeCommandService;
    private readonly DoneMaintenanceController _controller;

    public DoneMaintenanceControllerTests()
    {
        _fakeQueryService = A.Fake<IDoneMaintenanceQueryServices>();
        _fakeCommandService = A.Fake<IDoneMaintenanceCommandServices>();
        _controller = new DoneMaintenanceController(_fakeQueryService, _fakeCommandService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateDoneMaintenance_ShouldReturnOkResult()
    {
        // Arrange
        var maintenanceDto = new DoneMaintenanceDto
        {
            Id = 1,
            Type = "Test Maintenance"
        };

        A.CallTo(() => _fakeCommandService.CreateAsync(maintenanceDto))
            .Returns(Task.FromResult(maintenanceDto));

        // Act
        var result = await _controller.CreateDoneMaintenance(maintenanceDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Done Maintenance added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(maintenanceDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateDoneMaintenance_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var maintenanceDto = new DoneMaintenanceDto();
        var expectedException = new InvalidOperationException("Creation failed");

        A.CallTo(() => _fakeCommandService.CreateAsync(maintenanceDto))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.CreateDoneMaintenance(maintenanceDto));
    }

    [Fact]
    public async Task FinalizeMaintenance_ShouldReturnOkResult()
    {
        // Arrange
        var finalizeDto = new FinalizeMaintenanceDto
        {
            MaintenanceId = 1
        };

        A.CallTo(() => _fakeCommandService.FinalizeMaintenanceAsync(finalizeDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.FinalizeMaintenance(finalizeDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Maintenance process finished successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.FinalizeMaintenanceAsync(finalizeDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateDoneMaintenance_ShouldReturnOkResult()
    {
        // Arrange
        var maintenanceDto = new DoneMaintenanceDto
        {
            Id = 1,
            Type = "Updated Maintenance"
        };

        A.CallTo(() => _fakeCommandService.UpdateAsync(maintenanceDto))
            .Returns(Task.FromResult(maintenanceDto));

        // Act
        var result = await _controller.UpdateDoneMaintenance(maintenanceDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(maintenanceDto, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(maintenanceDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteDoneMaintenance_ShouldReturnOkResult()
    {
        // Arrange
        int maintenanceId = 1;

        A.CallTo(() => _fakeCommandService.DeleteAsync(maintenanceId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteDoneMaintenance(maintenanceId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Done Maintenance deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(maintenanceId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetDoneMaintenanceById_ShouldReturnOkResultWithMaintenance()
    {
        // Arrange
        int maintenanceId = 1;
        var expectedMaintenance = new GetDoneMaintenanceDto
        {
            Id = maintenanceId,
            Type = "Test Maintenance"
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(maintenanceId))
            .Returns(Task.FromResult(expectedMaintenance));

        // Act
        var result = await _controller.GetDoneMaintenanceById(maintenanceId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedMaintenance, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(maintenanceId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetDoneMaintenanceById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int maintenanceId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(maintenanceId))
            .Returns(Task.FromResult<GetDoneMaintenanceDto>(null!));

        // Act
        var result = await _controller.GetDoneMaintenanceById(maintenanceId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Done Maintenance with ID {maintenanceId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedDoneMaintenance_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
       
        var expectedResult = new PagedResultDto< GetDoneMaintenanceDto>
        {
            Items = new List< GetDoneMaintenanceDto>(),
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

        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetPagedDoneMaintenance(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetDoneMaintenanceByTechnicianId_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        int technicianId = 1;
        var expectedResult = new PagedResultDto<GetDoneMaintenanceDto>();

        A.CallTo(() => _fakeQueryService.GetByTechnicianIdAsync(pagedRequest, technicianId))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetDoneMaintenanceByTechnicianId(pagedRequest, technicianId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByTechnicianIdAsync(pagedRequest, technicianId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion
}