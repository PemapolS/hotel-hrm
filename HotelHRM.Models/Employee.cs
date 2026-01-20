namespace HotelHRM.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public decimal BaseSalary { get; set; }
    public EmploymentStatus Status { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

public enum EmploymentStatus
{
    Active,
    OnLeave,
    Terminated
}
