using HotelHRM.Models;
using System.Security.Cryptography;
using System.Text;

namespace HotelHRM.Data.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public InMemoryUserRepository()
    {
        // Seed with sample users
        // Password for all users: "password"
        var passwordHash = HashPassword("password");
        
        _users.AddRange(new[]
        {
            new User
            {
                Id = _nextId++,
                Username = "hr.admin",
                PasswordHash = passwordHash,
                Email = "hr.admin@hotelhrm.com",
                Role = UserRole.HR,
                EmployeeId = null,
                IsActive = true
            },
            new User
            {
                Id = _nextId++,
                Username = "john.doe",
                PasswordHash = passwordHash,
                Email = "john.doe@hotelhrm.com",
                Role = UserRole.Employee,
                EmployeeId = 1,
                IsActive = true
            },
            new User
            {
                Id = _nextId++,
                Username = "jane.smith",
                PasswordHash = passwordHash,
                Email = "jane.smith@hotelhrm.com",
                Role = UserRole.Employee,
                EmployeeId = 2,
                IsActive = true
            },
            new User
            {
                Id = _nextId++,
                Username = "michael.johnson",
                PasswordHash = passwordHash,
                Email = "michael.johnson@hotelhrm.com",
                Role = UserRole.Employee,
                EmployeeId = 3,
                IsActive = true
            }
        });
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users.ToList());
    }

    public Task<User?> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user)
    {
        var index = _users.FindIndex(u => u.Id == user.Id);
        if (index != -1)
        {
            _users[index] = user;
        }
        return Task.FromResult(user);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
