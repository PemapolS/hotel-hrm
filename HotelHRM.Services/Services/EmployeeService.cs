using HotelHRM.Data.Repositories;
using HotelHRM.Models;

namespace HotelHRM.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return _employeeRepository.GetAllAsync();
    }

    public Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return _employeeRepository.GetByIdAsync(id);
    }

    public Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        // Business logic validation can be added here
        return _employeeRepository.AddAsync(employee);
    }

    public Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        // Business logic validation can be added here
        return _employeeRepository.UpdateAsync(employee);
    }

    public Task<bool> DeleteEmployeeAsync(int id)
    {
        return _employeeRepository.DeleteAsync(id);
    }
}
