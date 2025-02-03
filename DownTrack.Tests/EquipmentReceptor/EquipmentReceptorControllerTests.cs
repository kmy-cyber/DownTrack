using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

public class EquipmentReceptorControllerTests
{
    private readonly IEquipmentReceptorQueryServices _fakeQueryService;
    private readonly EquipmentReceptorController _controller;

    public EquipmentReceptorControllerTests()
    {
        _fakeQueryService = A.Fake<IEquipmentReceptorQueryServices>();
        _controller = new EquipmentReceptorController(_fakeQueryService);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnOkResultWithEquipment()
    {
        // Arrange
        int equipmentId = 1;
        var expectedEquipment = new GetEquipmentReceptorDto
        {
            Id = equipmentId
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
            .Returns(Task.FromResult<GetEquipmentReceptorDto>(null!));

        // Act
        var result = await _controller.GetUserById(equipmentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"EquipmentReceptor with ID {equipmentId} not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPagedEquipmentReceptor_ShouldReturnOkResultWithPagedData()
    {
        // Arrange
        var pagedRequest = new PagedRequestDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        
        var expectedResult = new PagedResultDto<GetEquipmentReceptorDto>
        {
            Items = new List<GetEquipmentReceptorDto>(),
            TotalCount = 0,
            PageSize = 10,
            PageNumber = 1
        };

        // Mock HttpContext
        var httpContext = A.Fake<HttpContext>();
        var httpRequest = A.Fake<HttpRequest>();
        var httpScheme = "https";
        var httpHost = new HostString("localhost:5001");
        var httpPath = new PathString("/api/EquipmentReceptor/GetPaged");

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
        var result = await _controller.GetPagedEquipmentReceptor(pagedRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(pagedRequest))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllEquipmentReceptor_ShouldReturnOkResultWithAllData()
    {
        // Arrange
        var expectedResult = new List<GetEquipmentReceptorDto>
        {
            new GetEquipmentReceptorDto { Id = 1 },
            new GetEquipmentReceptorDto { Id = 2 }
        };

        A.CallTo(() => _fakeQueryService.ListAsync())
            .Returns(Task.FromResult((IEnumerable<GetEquipmentReceptorDto>)expectedResult));

        // Act
        var result = await _controller.GetAllEquipmentReceptor();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        A.CallTo(() => _fakeQueryService.ListAsync())
            .MustHaveHappenedOnceExactly();
    }
}