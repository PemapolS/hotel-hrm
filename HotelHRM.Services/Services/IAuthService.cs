using HotelHRM.Models;

namespace HotelHRM.Services.Services;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User?> GetCurrentUserAsync();
    Task LoginAsync(User user);
    Task LogoutAsync();
    bool IsInRole(UserRole role);
    bool CanModifyEmployeeData();
    bool CanModifyPayrollData();
}
