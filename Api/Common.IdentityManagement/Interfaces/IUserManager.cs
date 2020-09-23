using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.IdentityManagement.Interfaces
{
    public interface IUserManager
    {
        Task<bool> CreateAsync(User user, string password);
        Task<bool> AddToRoleAsync(int userId, string roleId);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<User> FindAsync(string userName, string password);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(int userId);
        Task<User> FindByNameAsync(string userName);
    }
}
