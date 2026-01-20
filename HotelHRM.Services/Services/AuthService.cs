using HotelHRM.Data.Repositories;
using HotelHRM.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace HotelHRM.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private User? _currentUser;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || !user.IsActive)
        {
            return null;
        }

        var passwordHash = InMemoryUserRepository.HashPassword(password);
        if (user.PasswordHash != passwordHash)
        {
            return null;
        }

        _currentUser = user;
        return user;
    }

    public Task<User?> GetCurrentUserAsync()
    {
        return Task.FromResult(_currentUser);
    }

    public Task LoginAsync(User user)
    {
        _currentUser = user;
        return Task.CompletedTask;
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        return Task.CompletedTask;
    }

    public bool IsInRole(UserRole role)
    {
        return _currentUser?.Role == role;
    }

    public bool CanModifyEmployeeData()
    {
        return _currentUser?.Role == UserRole.HR || _currentUser?.Role == UserRole.Admin;
    }

    public bool CanModifyPayrollData()
    {
        return _currentUser?.Role == UserRole.HR || _currentUser?.Role == UserRole.Admin;
    }
}
