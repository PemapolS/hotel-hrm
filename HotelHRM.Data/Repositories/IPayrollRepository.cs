using HotelHRM.Models;

namespace HotelHRM.Data.Repositories;

public interface IPayrollRepository
{
    Task<IEnumerable<PayrollRecord>> GetAllAsync();
    Task<PayrollRecord?> GetByIdAsync(int id);
    Task<IEnumerable<PayrollRecord>> GetByEmployeeIdAsync(int employeeId);
    Task<PayrollRecord> AddAsync(PayrollRecord payrollRecord);
    Task<PayrollRecord> UpdateAsync(PayrollRecord payrollRecord);
}
