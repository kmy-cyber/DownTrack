
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRespository;
using AutoMapper;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to employee and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class EmployeeServices : IEmployeeServices
{

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeServices(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }



    /// <summary>
    /// Creates a new employee based on the provided EmployeeDto.
    /// </summary>
    /// <param name="employeeDto">The DTO containing employee details to be created.</param>
    /// <returns>A Task representing the asynchronous operation, with an EmployeeDto as the result.</returns>
    public async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
    {
        // map the DTOs (employeeDto) to a domain entity (Employee) 
        var result = _mapper.Map<Employee>(employeeDto);

        // method of the repository is called to insert the Employee entity into the database
        await _employeeRepository.CreateAsync(result);

        // PONER AQUI EL GENERADOR DE TOKEN


        // map the new created Employee entity to a EmployeeDTO
        return _mapper.Map<EmployeeDto>(result);
    }



    /// <summary>
    /// Updates an existing employee's information.
    /// </summary>
    /// <param name="employeeDto">The DTO containing updated employee details.</param>
    /// <returns>A Task representing the asynchronous operation, with an EmployeeDto as the result.</returns>
    public async Task<EmployeeDto> UpdateAsync(EmployeeDto employeeDto)
    {
        var result = _employeeRepository.GetById(employeeDto.Id);

        // Maps the provided EmployeeDto to the existing employee entity, updates the employee in the database, 
        _mapper.Map(employeeDto, result);

        await _employeeRepository.UpdateAsync(result);

        /// and returns the updated employee as a EmployeeDto.
        return _mapper.Map<EmployeeDto>(result);
    }



    /// <summary>
    /// Retrieves a list of all employees.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of EmployeeDto as the result.</returns>
    public async Task<IEnumerable<EmployeeDto>> ListAsync()
    {
        //fetches all employees from the repository
        var results = await _employeeRepository.ListAsync();

        //return them as a enumerable of TechnicanDto objects
        return results.Select(_mapper.Map<EmployeeDto>);

    }



    /// <summary>
    /// Deletes a employee by its ID.
    /// </summary>
    /// <param name="employeeDto">The employee's ID to be deleted.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(int employeeDto)
    {
        await _employeeRepository.DeleteByIdAsync(employeeDto);
    }



    /// <summary>
    /// Retrieves a employee by their ID
    /// </summary>
    /// <param name="employeeDto">The employee's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the employee</returns>
    public async Task<EmployeeDto> GetByIdAsync(int employeeDto)
    {
        var result = await _employeeRepository.GetByIdAsync(employeeDto);

        /// and returns the updated employee as a EmployeeDto.
        return _mapper.Map<EmployeeDto>(result);

    }



}