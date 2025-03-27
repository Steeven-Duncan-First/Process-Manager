using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSource.Mock.Models;

namespace GoldenSource.Mock.Services
{
    public interface IMockUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> SearchUsersAsync(string query);
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> HasRoleAsync(string userId, string role);
        Task<List<AccessLog>> GetUserAccessLogsAsync(string userId);
    }

    public class MockUserService : IMockUserService
    {
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // Simuler un délai réseau
            await Task.Delay(500);

            // Dans un environnement mock, on accepte n'importe quel mot de passe
            return MockData.Users.FirstOrDefault(u => u.UserName == username);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            return MockData.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string query)
        {
            // Simuler un délai réseau
            await Task.Delay(500);

            var searchTerms = query.ToLower().Split(' ');
            return MockData.Users
                .Where(u => u.IsActive &&
                           searchTerms.Any(term =>
                               u.FirstName.ToLower().Contains(term) ||
                               u.LastName.ToLower().Contains(term) ||
                               u.UserName.ToLower().Contains(term) ||
                               u.Department.ToLower().Contains(term) ||
                               u.Service.ToLower().Contains(term)))
                .ToList();
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(string department)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            return MockData.Users
                .Where(u => u.IsActive && u.Department.ToLower() == department.ToLower())
                .ToList();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Simuler un délai réseau
            await Task.Delay(600);

            var existingUser = MockData.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Department = user.Department;
                existingUser.Service = user.Service;
                existingUser.Function = user.Function;
                existingUser.MaxAccessLevel = user.MaxAccessLevel;
                existingUser.Roles = user.Roles;
                existingUser.IsActive = user.IsActive;
                existingUser.LastLoginDate = DateTime.Now;
                return true;
            }
            return false;
        }

        public async Task<bool> HasRoleAsync(string userId, string role)
        {
            // Simuler un délai réseau
            await Task.Delay(200);

            var user = MockData.Users.FirstOrDefault(u => u.Id == userId);
            return user != null && user.Roles.Split(',').Select(r => r.Trim()).Contains(role);
        }

        public async Task<List<AccessLog>> GetUserAccessLogsAsync(string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(400);

            return MockData.AccessLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.AccessDate)
                .Take(50)
                .ToList();
        }
    }
} 