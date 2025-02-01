using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using AutoMapper;
using DownTrack.Domain.Entities;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to employee and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class EmployeeCommandServices : IEmployeeCommandServices
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    /// <summary>
    /// Creates a new employee based on the provided EmployeeDto.
    /// </summary>
    /// <param name="employeeDto">The DTO containing employee details to be created.</param>
    /// <returns>A Task representing the asynchronous operation, with an EmployeeDto as the result.</returns>
    public async Task<EmployeeDto> CreateAsync(EmployeeDto dto)
    {
        Employee employee = _mapper.Map<Employee>(dto);
        
        if(employee.UserRole != UserRole.ShippingSupervisor.ToString())
        {
            throw new Exception("This user is not a Shipping Supervisor");
        }
        await _unitOfWork.GetRepository<Employee>().CreateAsync(employee);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EmployeeDto>(employee);
    }



    /// <summary>
    /// Updates an existing employee's information.
    /// </summary>
    /// <param name="employeeDto">The DTO containing updated employee details.</param>
    /// <returns>A Task representing the asynchronous operation, with an EmployeeDto as the result.</returns>
    public async Task<EmployeeDto> UpdateAsync(EmployeeDto dto)
    {
        var employee = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(dto.Id);

        //var employee = _employeeRepository.GetById(dto.Id);
        _mapper.Map(dto, employee);

        _unitOfWork.GetRepository<Employee>().Update(employee);

        await _unitOfWork.CompleteAsync();
        //await _employeeRepository.UpdateAsync(employee);
        return _mapper.Map<EmployeeDto>(employee);
    }



    /// <summary>
    /// Deletes a employee by its ID.
    /// </summary>
    /// <param name="employeeDto">The employee's ID to be deleted.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(int employeeId)
    {
        var employee = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(employeeId);

        await _unitOfWork.GetRepository<Employee>().DeleteByIdAsync(employeeId);

        if(employee.UserRole != UserRole.ShippingSupervisor.ToString())
            await _unitOfWork.UserRepository.DeleteByIdAsync(employeeId);

        await _unitOfWork.CompleteAsync();
    }


}
