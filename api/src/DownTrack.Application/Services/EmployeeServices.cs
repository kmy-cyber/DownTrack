
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using AutoMapper;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to employee and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class EmployeeServices : IEmployeeServices
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeServices(IUnitOfWork unitOfWork, IMapper mapper)
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
    /// Retrieves a list of all employees.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of EmployeeDto as the result.</returns>
    public async Task<IEnumerable<EmployeeDto>> ListAsync()
    {
        var employee = await _unitOfWork.GetRepository<Employee>().GetAllAsync().ToListAsync();
        //var employee = await _employeeRepository.ListAsync();
        return employee.Select(_mapper.Map<EmployeeDto>);
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



    /// <summary>
    /// Retrieves a employee by their ID
    /// </summary>
    /// <param name="employeeDto">The employee's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the employee</returns>
    public async Task<EmployeeDto> GetByIdAsync(int employeeDto)
    {
        var result = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(employeeDto);
        
        //var result = await _employeeRepository.GetByIdAsync(employeeDto);

        /// and returns the updated employee as an EmployeeDto.
        return _mapper.Map<EmployeeDto>(result);

    }



}
