using HotelHRM.Models;
using HotelHRM.Services.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace HotelHRM.Web.Authentication;

public class AuthServiceWrapper
{
    private readonly IAuthService _authService;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthServiceWrapper(IAuthService authService, AuthenticationStateProvider authStateProvider)
    {
        _authService = authService;
        _authStateProvider = authStateProvider;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        return await _authService.AuthenticateAsync(username, password);
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var userClaim = authState.User;

        if (userClaim?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var username = userClaim.Identity.Name;
        if (string.IsNullOrEmpty(username))
        {
            return null;
        }

        var user = await _authService.AuthenticateAsync(username, string.Empty);
        return user;
    }

    public async Task LoginAsync(User user)
    {
        await _authService.LoginAsync(user);
        
        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            await customProvider.MarkUserAsAuthenticated(user);
        }
    }

    public async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        
        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            await customProvider.MarkUserAsLoggedOut();
        }
    }

    public bool CanModifyEmployeeData()
    {
        return _authService.CanModifyEmployeeData();
    }

    public bool CanModifyPayrollData()
    {
        return _authService.CanModifyPayrollData();
    }
}
