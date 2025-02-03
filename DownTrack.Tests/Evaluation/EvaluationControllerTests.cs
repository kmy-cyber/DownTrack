using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class EvaluationControllerTests
{
    private readonly IEvaluationQueryServices _fakeQueryService;
    private readonly IEvaluationCommandServices _fakeCommandService;
    private readonly EvaluationController _controller;

    public EvaluationControllerTests()
    {
        _fakeQueryService = A.Fake<IEvaluationQueryServices>();
        _fakeCommandService = A.Fake<IEvaluationCommandServices>();
        _controller = new EvaluationController(_fakeQueryService, _fakeCommandService);
    }

    #region Command Tests

    [Fact]
    public async Task CreateEvaluation_ShouldReturnOkResult()
    {
        // Arrange
        var evaluationDto = new EvaluationDto
        {
            Id = 1
        };

        A.CallTo(() => _fakeCommandService.CreateAsync(evaluationDto))
            .Returns(Task.FromResult(evaluationDto));

        // Act
        var result = await _controller.CreateEvaluation(evaluationDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Evaluation added successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.CreateAsync(evaluationDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateEvaluation_ShouldReturnOkResult()
    {
        // Arrange
        var evaluationDto = new EvaluationDto
        {
            Id = 1
        };

        A.CallTo(() => _fakeCommandService.UpdateAsync(evaluationDto))
            .Returns(Task.FromResult(evaluationDto));

        // Act
        var result = await _controller.UpdateEvaluation(evaluationDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(evaluationDto, okResult.Value);
        A.CallTo(() => _fakeCommandService.UpdateAsync(evaluationDto))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteEvaluation_ShouldReturnOkResult()
    {
        // Arrange
        int evaluationId = 1;

        A.CallTo(() => _fakeCommandService.DeleteAsync(evaluationId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteEvaluation(evaluationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Evaluation deleted successfully", okResult.Value);
        A.CallTo(() => _fakeCommandService.DeleteAsync(evaluationId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Query Tests

    [Fact]
    public async Task GetUserById_ShouldReturnOkResultWithEvaluation()
    {
        // Arrange
        int evaluationId = 1;
        var expectedEvaluation = new GetEvaluationDto
        {
            Id = evaluationId
        };

        A.CallTo(() => _fakeQueryService.GetByIdAsync(evaluationId))
            .Returns(Task.FromResult(expectedEvaluation));

        // Act
        var result = await _controller.GetUserById(evaluationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedEvaluation, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetByIdAsync(evaluationId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetUserById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        int evaluationId = 999;

        A.CallTo(() => _fakeQueryService.GetByIdAsync(evaluationId))
            .Returns(Task.FromResult<GetEvaluationDto>(null!));

        // Act
        var result = await _controller.GetUserById(evaluationId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Evaluation with ID {evaluationId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedEvaluation_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        
        var expectedResult = new PagedResultDto<GetEvaluationDto>
        {
            Items = new List<GetEvaluationDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/Evaluation/GetPaged");

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
        var result = await _controller.GetPagedEvaluation(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .MustHaveHappenedOnceExactly();
    }

    #endregion
}