using HotelHRM.Data.Repositories;
using HotelHRM.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace HotelHRM.Web.Authentication;

public class WebAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthenticationStateProvider _authStateProvider;

    public WebAuthService(IUserRepository userRepository, AuthenticationStateProvider authStateProvider)
    {
        _userRepository = userRepository;
        _authStateProvider = authStateProvider;
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

        return user;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var userClaim = authState.User;

        if (!userClaim.Identity?.IsAuthenticated ?? true)
        {
            return null;
        }

        var username = userClaim.Identity.Name;
        if (string.IsNullOrEmpty(username))
        {
            return null;
        }

        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task LoginAsync(User user)
    {
        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            await customProvider.MarkUserAsAuthenticated(user);
        }
    }

    public async Task LogoutAsync()
    {
        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            await customProvider.MarkUserAsLoggedOut();
        }
    }

    public async Task<bool> IsInRoleAsync(UserRole role)
    {
        var user = await GetCurrentUserAsync();
        return user?.Role == role;
    }

    public async Task<bool> CanModifyEmployeeDataAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.Role == UserRole.HR || user?.Role == UserRole.Admin;
    }

    public async Task<bool> CanModifyPayrollDataAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.Role == UserRole.HR || user?.Role == UserRole.Admin;
    }
}
