using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class TransferRequestControllerTests
{
    private readonly ITransferRequestQueryServices _fakeQueryService;
    private readonly ITransferRequestCommandServices _fakeCommandService;
    private readonly TransferRequestController _controller;

    public TransferRequestControllerTests()
    {
        _fakeQueryService = A.Fake<ITransferRequestQueryServices>();
        _fakeCommandService = A.Fake<ITransferRequestCommandServices>();
        _controller = new TransferRequestController(_fakeQueryService, _fakeCommandService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateTransferRequest_ShouldReturnOkResult()
    {
        // Arrange
        var transferRequestDto = new TransferRequestDto
        {
            Id = 1,
            // Add other required properties
        };

        A.CallTo(() => _fakeCommandService.CreateAsync(transferRequestDto))
            .Returns(Task.FromResult(transferRequestDto));

        // Act
        var result = await _controller.CreateTransferRequest(transferRequestDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("TransferRequest added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(transferRequestDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateTransferRequest_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var transferRequestDto = new TransferRequestDto();
        var expectedException = new InvalidOperationException("Creation failed");

        A.CallTo(() => _fakeCommandService.CreateAsync(transferRequestDto))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.CreateTransferRequest(transferRequestDto));
    }

    [Fact]
    public async Task UpdateTransferRequest_ShouldReturnOkResult()
    {
        // Arrange
        var transferRequestDto = new TransferRequestDto
        {
            Id = 1,
            // Add other required properties
        };

        A.CallTo(() => _fakeCommandService.UpdateAsync(transferRequestDto))
            .Returns(Task.FromResult(transferRequestDto));

        // Act
        var result = await _controller.UpdateTransferRequest(transferRequestDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(transferRequestDto, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(transferRequestDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteTransferRequest_ShouldReturnOkResult()
    {
        // Arrange
        int transferRequestId = 1;

        A.CallTo(() => _fakeCommandService.DeleteAsync(transferRequestId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTransferRequest(transferRequestId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("TransferRequest deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(transferRequestId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetTransferRequestById_ShouldReturnOkResultWithTransferRequest()
    {
        // Arrange
        int transferRequestId = 1;
        var expectedTransferRequest = new GetTransferRequestDto
        {
            Id = transferRequestId,
            // Add other required properties
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(transferRequestId))
            .Returns(Task.FromResult(expectedTransferRequest));

        // Act
        var result = await _controller.GetTransferRequestById(transferRequestId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedTransferRequest, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(transferRequestId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetTransferRequestById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int transferRequestId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(transferRequestId))
            .Returns(Task.FromResult<GetTransferRequestDto>(null!));

        // Act
        var result = await _controller.GetTransferRequestById(transferRequestId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"TransferRequest with ID {transferRequestId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedUser_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        var expectedResult = new PagedResultDto<GetTransferRequestDto>
        {
            Items = new List<GetTransferRequestDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/TransferRequest/GetPaged");

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
        var result = await _controller.GetPagedUser(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetTransferRequestByDepartment_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        int receptorId = 1;
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        var expectedResult = new PagedResultDto<GetTransferRequestDto>
        {
            Items = new List<GetTransferRequestDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/TransferRequest/GetByArrivalDepartment/1");

        A.CallTo(() => httpRequest.Scheme).Returns(httpScheme);
        A.CallTo(() => httpRequest.Host).Returns(httpHost);
        A.CallTo(() => httpRequest.Path).Returns(httpPath);
        A.CallTo(() => httpContext.Request).Returns(httpRequest);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        A.CallTo(() => _fakeQueryService.GetPagedRequestsofArrivalDepartmentAsync(receptorId, pagedRequest))
            .Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _controller.GetTransferRequestByDepartment(receptorId, pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetPagedRequestsofArrivalDepartmentAsync(receptorId, pagedRequest))
            .MustHaveHappenedOnceExactly();
    }

    #endregion
}