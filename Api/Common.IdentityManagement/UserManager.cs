using Common.DataAccess;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;
using Common.IdentityManagement.Interfaces;
using System.Security.Cryptography;

namespace Common.IdentityManagement
{
    public class UserManager: IUserManager
    {
        private readonly IUnitOfWork uow;
        public UserManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        //
        // Summary:
        //     Create a user with the given password
        //
        // Parameters:
        //   user:
        //
        //   password:
        public async Task<bool> CreateAsync(User user, string password)
        {
            try
            {
                this.uow.BeginTran();

                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                var saltStr = Convert.ToBase64String(salt);
                var passwordHashed = Security.PasswordHash.Create(salt, password);

                user.Salt = saltStr;
                user.PasswordHashed = passwordHashed;

                var userId = await this.uow.UserRepository.Insert(user);

                this.uow.CommitTran();

                return userId > 0;
            }
            catch (Exception ex)
            {
                this.uow.RollBackTran();
                throw ex;
            }
        }
        public async Task<bool> AddToRoleAsync(int userId, string roleId)
        {
            await this.uow.UserRoleRepository.Insert(new UserRole() { UserId = userId, RoleId = roleId });
            return await Task.FromResult(true);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            byte[] salt = Convert.FromBase64String(user.Salt);
            var passwordHashed = Security.PasswordHash.Create(salt, password);
            bool isValid = user.PasswordHashed.Equals(passwordHashed);
            return await Task.FromResult(isValid);
        }
        //
        // Summary:
        //     Change a user password
        //
        // Parameters:
        //   userId:
        //
        //   currentPassword:
        //
        //   newPassword:
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(newPassword))
                    return await Task.FromResult(false);

                this.uow.BeginTran();
                var user = await this.uow.UserRepository.FindByIdAsync(userId);
                var salt = Convert.FromBase64String(user.Salt);
                user.PasswordHashed = Security.PasswordHash.Create(salt, newPassword);
                await this.uow.UserRepository.Update(user);
                this.uow.CommitTran();

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                this.uow.RollBackTran();
                throw ex;
            }
        }
        //
        // Summary:
        //     Return a user with the specified username and password or null if there is no
        //     match.
        //
        // Parameters:
        //   userName:
        //
        //   password:
        public Task<User> FindAsync(string userName, string password) => this.uow.UserRepository.FindAsync(userName, password);
        //
        // Summary:
        //     Find a user by his email
        //
        // Parameters:
        //   email:
        public Task<User> FindByEmailAsync(string email) => this.uow.UserRepository.FindByEmailAsync(email);
        //
        // Summary:
        //     Find a user by id
        //
        // Parameters:
        //   userId:
        public Task<User> FindByIdAsync(int userId) => this.uow.UserRepository.FindByIdAsync(userId);
        //
        // Summary:
        //     Find a user by user name
        //
        // Parameters:
        //   userName:
        public Task<User> FindByNameAsync(string userName) => this.uow.UserRepository.FindByLoginAsync(userName);
    }
}
