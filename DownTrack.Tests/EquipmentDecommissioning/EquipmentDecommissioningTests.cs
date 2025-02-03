using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Api.Controllers;
using System.Threading.Tasks;

namespace DownTrack.Api.Tests.Controllers
{
    public class EquipmentDecommissioningControllerTests
    {
        private readonly IEquipmentDecommissioningQueryServices _fakeQueryService;
        private readonly IEquipmentDecommissioningCommandServices _fakeCommandService;
        private readonly EquipmentDecommissioningController _controller;

        public EquipmentDecommissioningControllerTests()
        {
            _fakeQueryService = A.Fake<IEquipmentDecommissioningQueryServices>();
            _fakeCommandService = A.Fake<IEquipmentDecommissioningCommandServices>();
            _controller = new EquipmentDecommissioningController(_fakeQueryService, _fakeCommandService);
        }

        private EquipmentDecommissioningDto CreateSampleDto(int id = 1)
        {
            return new EquipmentDecommissioningDto
            {
                Id = id,
                TechnicianId = id + 100,
                EquipmentId = id + 200,
                ReceptorId = id + 300,
                Date = DateTime.Now,
                Cause = $"Test Cause {id}",
                Status = "Pending"
            };
        }
        private GetEquipmentDecommissioningDto GetSampleDto(int id = 1)
        {
            return new GetEquipmentDecommissioningDto
            {
                Id = id,
                TechnicianId = id + 100,
                EquipmentId = id + 200,
                ReceptorId = id + 300,
                Date = DateTime.Now,
                Cause = $"Test Cause {id}",
                Status = "Pending"
            };
        }

        #region Command Tests

        [Fact]
        public async Task CreateEquipmentDecommissioning_ShouldReturnOkResult()
        {
            // Arrange
            var decommissioningDto = CreateSampleDto();

            A.CallTo(() => _fakeCommandService.CreateAsync(decommissioningDto))
                .Returns(Task.FromResult(decommissioningDto));

            // Act
            var result = await _controller.CreateEquipmentDecommissioning(decommissioningDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Equipment Decommissioning added successfully", okResult.Value);
            A.CallTo(() => _fakeCommandService.CreateAsync(decommissioningDto))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateEquipmentDecommissioning_ShouldReturnOkResult()
        {
            // Arrange
            var decommissioningDto = CreateSampleDto();
            decommissioningDto.Status = "In Progress";
            decommissioningDto.Cause = "Updated Cause";

            A.CallTo(() => _fakeCommandService.UpdateAsync(decommissioningDto))
                .Returns(Task.FromResult(decommissioningDto));

            // Act
            var result = await _controller.UpdateEquipmentDecommissioning(decommissioningDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Equipment Decommissioning updated successfully", okResult.Value);
            A.CallTo(() => _fakeCommandService.UpdateAsync(decommissioningDto))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteEquipmentDecommissioning_ShouldReturnOkResult()
        {
            // Arrange
            int decommissioningId = 1;

            A.CallTo(() => _fakeCommandService.DeleteAsync(decommissioningId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEquipmentDecommissioning(decommissioningId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Equipment Decommissioning deleted successfully", okResult.Value);
            A.CallTo(() => _fakeCommandService.DeleteAsync(decommissioningId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AcceptDecommissioning_ShouldReturnOkResult()
        {
            // Arrange
            int decommissioningId = 1;

            A.CallTo(() => _fakeCommandService.AcceptDecommissioningAsync(decommissioningId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AcceptDecommissioning(decommissioningId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Equipment Decommissioning accepted successfully", okResult.Value);
            A.CallTo(() => _fakeCommandService.AcceptDecommissioningAsync(decommissioningId))
                .MustHaveHappenedOnceExactly();
        }

        #endregion

        #region Query Tests

        [Fact]
        public async Task GetEquipmentDecommissioningById_ShouldReturnOkResultWithDecommissioning()
        {
            // Arrange
            int decommissioningId = 1;
            var expectedDecommissioning = new GetEquipmentDecommissioningDto (){

            };

            A.CallTo(() => _fakeQueryService.GetByIdAsync(decommissioningId))
                .Returns(Task.FromResult(expectedDecommissioning));

            // Act
            var result = await _controller.GetEquipmentDecommissioningById(decommissioningId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedDecommissioning, okResult.Value);
            A.CallTo(() => _fakeQueryService.GetByIdAsync(decommissioningId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetEquipmentDecommissioningById_WhenNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int decommissioningId = 999;

            A.CallTo(() => _fakeQueryService.GetByIdAsync(decommissioningId))
                .Returns(Task.FromResult<GetEquipmentDecommissioningDto>(null!));

            // Act
            var result = await _controller.GetEquipmentDecommissioningById(decommissioningId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Equipment Decommissioning with ID {decommissioningId} not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllEquipmentDecommissioning_ShouldReturnOkResultWithList()
        {
            // Arrange
            var expectedList = new List<GetEquipmentDecommissioningDto>
            {
                GetSampleDto(1),
                GetSampleDto(2),
                GetSampleDto(3)
            };

            A.CallTo(() => _fakeQueryService.ListAsync())
                .Returns(Task.FromResult<IEnumerable<GetEquipmentDecommissioningDto>>(expectedList));

            // Act
            var result = await _controller.GetAllEquipmentDecommissioning();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedList, okResult.Value);
            A.CallTo(() => _fakeQueryService.ListAsync())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetPagedAllDepartmentInSection_ShouldReturnOkResultWithPagedData()
        {
            // Arrange
            var pagedRequest = new PagedRequestDto
            {
                PageNumber = 1,
                PageSize = 10
            };

            var expectedResult = new PagedResultDto<GetEquipmentDecommissioningDto>
            {
                Items = new List<GetEquipmentDecommissioningDto>
                {
                    GetSampleDto(1),
                    GetSampleDto(2),
                    GetSampleDto(3)
                },
                TotalCount = 3,
                PageSize = 10,
                PageNumber = 1
            };

            // Mock the HttpContext
            var httpContext = A.Fake<HttpContext>();
            var httpRequest = A.Fake<HttpRequest>();
            var httpScheme = "https";
            var httpHost = new HostString("localhost:5001");
            var httpPath = new PathString("/api/EquipmentDecommissioning/Get_Paged_All");

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
            var result = await _controller.GetPagedAllDepartmentInSection(pagedRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
            A.CallTo(() => _fakeQueryService.GetAllPagedResultAsync(A<PagedRequestDto>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetEquipmentDecomissioningByReceptorId_ShouldReturnOkResultWithPagedData()
        {
            // Arrange
            var pagedRequest = new PagedRequestDto
            {
                PageNumber = 1,
                PageSize = 10
            };
            int receptorId = 1;

            var expectedResult = new PagedResultDto<GetEquipmentDecommissioningDto>
            {
                Items = new List<GetEquipmentDecommissioningDto>
                {
                    GetSampleDto(1),
                    GetSampleDto(2)
                },
                TotalCount = 2,
                PageSize = 10,
                PageNumber = 1
            };

            // Mock the HttpContext
            var httpContext = A.Fake<HttpContext>();
            var httpRequest = A.Fake<HttpRequest>();
            var httpScheme = "https";
            var httpHost = new HostString("localhost:5001");
            var httpPath = new PathString($"/api/EquipmentDecommissioning/Get_Paged_All_By_ReceptorId/{receptorId}");

            A.CallTo(() => httpRequest.Scheme).Returns(httpScheme);
            A.CallTo(() => httpRequest.Host).Returns(httpHost);
            A.CallTo(() => httpRequest.Path).Returns(httpPath);
            A.CallTo(() => httpContext.Request).Returns(httpRequest);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            A.CallTo(() => _fakeQueryService.GetEquipmentDecomissioningOfReceptorAsync(receptorId, A<PagedRequestDto>._))
                .Returns(Task.FromResult(expectedResult));

            // Act
            var result = await _controller.GetEquipmentDecomissioningByReceptorId(receptorId, pagedRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
            A.CallTo(() => _fakeQueryService.GetEquipmentDecomissioningOfReceptorAsync(receptorId, A<PagedRequestDto>._))
                .MustHaveHappenedOnceExactly();
        }

        #endregion
    }
}