using HotelHRM.Data.Repositories;
using HotelHRM.Models;

namespace HotelHRM.Services.Services;

public class PayrollService : IPayrollService
{
    private readonly IPayrollRepository _payrollRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public PayrollService(IPayrollRepository payrollRepository, IEmployeeRepository employeeRepository)
    {
        _payrollRepository = payrollRepository;
        _employeeRepository = employeeRepository;
    }

    public Task<IEnumerable<PayrollRecord>> GetAllPayrollRecordsAsync()
    {
        return _payrollRepository.GetAllAsync();
    }

    public Task<PayrollRecord?> GetPayrollRecordByIdAsync(int id)
    {
        return _payrollRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<PayrollRecord>> GetPayrollRecordsByEmployeeIdAsync(int employeeId)
    {
        return _payrollRepository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<PayrollRecord> ProcessPayrollAsync(int employeeId, DateTime periodStart, DateTime periodEnd, decimal bonus, decimal deductions)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
        }

        // Calculate payroll
        var daysInPeriod = (periodEnd - periodStart).Days + 1;
        var monthlyRate = employee.BaseSalary / 12;
        var dailyRate = monthlyRate / 30;
        var baseSalary = dailyRate * daysInPeriod;

        var grossPay = baseSalary + bonus;
        var netPay = grossPay - deductions;

        var payrollRecord = new PayrollRecord
        {
            EmployeeId = employeeId,
            Employee = employee,
            PayPeriodStart = periodStart,
            PayPeriodEnd = periodEnd,
            BaseSalary = baseSalary,
            Bonus = bonus,
            Deductions = deductions,
            GrossPay = grossPay,
            NetPay = netPay,
            ProcessedDate = DateTime.Now,
            Status = PayrollStatus.Processed
        };

        return await _payrollRepository.AddAsync(payrollRecord);
    }
}
