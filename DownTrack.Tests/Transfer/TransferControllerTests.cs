using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class TransferControllerTests
{
    private readonly ITransferQueryServices _fakeQueryService;
    private readonly ITransferCommandServices _fakeCommandService;
    private readonly TransferController _controller;

    public TransferControllerTests()
    {
        _fakeQueryService = A.Fake<ITransferQueryServices>();
        _fakeCommandService = A.Fake<ITransferCommandServices>();
        _controller = new TransferController(_fakeQueryService, _fakeCommandService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateTransfer_ShouldReturnOkResult()
    {
        // Arrange
        var transferDto = new TransferDto
        {
            Id = 1,
            // Add other required properties
        };

        A.CallTo(() => _fakeCommandService.CreateAsync(transferDto))
            .Returns(Task.FromResult(transferDto));

        // Act
        var result = await _controller.CreateTransfer(transferDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Transfer added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(transferDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateTransfer_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var transferDto = new TransferDto();
        var expectedException = new InvalidOperationException("Creation failed");

        A.CallTo(() => _fakeCommandService.CreateAsync(transferDto))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.CreateTransfer(transferDto));
    }

    [Fact]
    public async Task UpdateTransfer_ShouldReturnOkResult()
    {
        // Arrange
        var transferDto = new TransferDto
        {
            Id = 1,
            // Add other required properties
        };

        A.CallTo(() => _fakeCommandService.UpdateAsync(transferDto))
            .Returns(Task.FromResult(transferDto));

        // Act
        var result = await _controller.UpdateTransfer(transferDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(transferDto, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(transferDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteTransfer_ShouldReturnOkResult()
    {
        // Arrange
        int transferId = 1;

        A.CallTo(() => _fakeCommandService.DeleteAsync(transferId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTransfer(transferId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Transfer deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(transferId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetTransferById_ShouldReturnOkResultWithTransfer()
    {
        // Arrange
        int transferId = 1;
        var expectedTransfer = new GetTransferDto
        {
            Id = transferId,
            // Add other required properties
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(transferId))
            .Returns(Task.FromResult(expectedTransfer));

        // Act
        var result = await _controller.GetTransferById(transferId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedTransfer, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(transferId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetTransferById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int transferId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(transferId))
            .Returns(Task.FromResult<GetTransferDto>(null!));

        // Act
        var result = await _controller.GetTransferById(transferId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Transfer with ID {transferId} not found", notFoundResult.Value);
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

        var expectedResult = new PagedResultDto<GetTransferDto>
        {
            Items = new List<GetTransferDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/Transfer/GetPaged");

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

    #endregion
}