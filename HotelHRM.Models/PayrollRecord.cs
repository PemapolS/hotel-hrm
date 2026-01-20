namespace HotelHRM.Models;

public class PayrollRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deductions { get; set; }
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public DateTime ProcessedDate { get; set; }
    public PayrollStatus Status { get; set; }
}

public enum PayrollStatus
{
    Pending,
    Processed,
    Paid
}
