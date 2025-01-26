using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DownTrack.Application.IServices;

public interface ISectionServices : IGenericService<SectionDto>
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartments(int sectionId);

    public Task<PagedResultDto<Section>> GetPagedSectionsByManagerIdAsync(
        int managerId, PagedRequestDto pagedRequest);
}