using HotelHRM.Models;

namespace HotelHRM.Data.Repositories;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees = new();
    private int _nextId = 1;

    public InMemoryEmployeeRepository()
    {
        // Seed with sample data
        _employees.AddRange(new[]
        {
            new Employee
            {
                Id = _nextId++,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@hotelhrm.com",
                PhoneNumber = "+1-555-0100",
                Department = "Front Desk",
                Position = "Receptionist",
                HireDate = new DateTime(2023, 1, 15),
                BaseSalary = 35000,
                Status = EmploymentStatus.Active
            },
            new Employee
            {
                Id = _nextId++,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@hotelhrm.com",
                PhoneNumber = "+1-555-0101",
                Department = "Housekeeping",
                Position = "Housekeeping Manager",
                HireDate = new DateTime(2022, 6, 1),
                BaseSalary = 45000,
                Status = EmploymentStatus.Active
            },
            new Employee
            {
                Id = _nextId++,
                FirstName = "Michael",
                LastName = "Johnson",
                Email = "michael.johnson@hotelhrm.com",
                PhoneNumber = "+1-555-0102",
                Department = "Food & Beverage",
                Position = "Chef",
                HireDate = new DateTime(2021, 3, 10),
                BaseSalary = 55000,
                Status = EmploymentStatus.Active
            }
        });
    }

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Employee>>(_employees.ToList());
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(employee);
    }

    public Task<Employee> AddAsync(Employee employee)
    {
        employee.Id = _nextId++;
        _employees.Add(employee);
        return Task.FromResult(employee);
    }

    public Task<Employee> UpdateAsync(Employee employee)
    {
        var index = _employees.FindIndex(e => e.Id == employee.Id);
        if (index != -1)
        {
            _employees[index] = employee;
        }
        return Task.FromResult(employee);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            _employees.Remove(employee);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
