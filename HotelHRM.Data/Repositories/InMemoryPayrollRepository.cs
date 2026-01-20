using HotelHRM.Models;

namespace HotelHRM.Data.Repositories;

public class InMemoryPayrollRepository : IPayrollRepository
{
    private readonly List<PayrollRecord> _payrollRecords = new();
    private int _nextId = 1;

    public Task<IEnumerable<PayrollRecord>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<PayrollRecord>>(_payrollRecords.ToList());
    }

    public Task<PayrollRecord?> GetByIdAsync(int id)
    {
        var record = _payrollRecords.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(record);
    }

    public Task<IEnumerable<PayrollRecord>> GetByEmployeeIdAsync(int employeeId)
    {
        var records = _payrollRecords.Where(p => p.EmployeeId == employeeId).ToList();
        return Task.FromResult<IEnumerable<PayrollRecord>>(records);
    }

    public Task<PayrollRecord> AddAsync(PayrollRecord payrollRecord)
    {
        payrollRecord.Id = _nextId++;
        _payrollRecords.Add(payrollRecord);
        return Task.FromResult(payrollRecord);
    }

    public Task<PayrollRecord> UpdateAsync(PayrollRecord payrollRecord)
    {
        var index = _payrollRecords.FindIndex(p => p.Id == payrollRecord.Id);
        if (index != -1)
        {
            _payrollRecords[index] = payrollRecord;
        }
        return Task.FromResult(payrollRecord);
    }
}
