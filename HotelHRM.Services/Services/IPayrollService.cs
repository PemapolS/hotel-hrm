using HotelHRM.Models;

namespace HotelHRM.Services.Services;

public interface IPayrollService
{
    Task<IEnumerable<PayrollRecord>> GetAllPayrollRecordsAsync();
    Task<PayrollRecord?> GetPayrollRecordByIdAsync(int id);
    Task<IEnumerable<PayrollRecord>> GetPayrollRecordsByEmployeeIdAsync(int employeeId);
    Task<PayrollRecord> ProcessPayrollAsync(int employeeId, DateTime periodStart, DateTime periodEnd, decimal bonus, decimal deductions);
}
