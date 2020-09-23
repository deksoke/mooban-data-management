using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Infrastructure.Repositories
{
    public interface IIdentityUserRepository<TUser> where TUser : User
    {
        Task Delete(int id);
        Task<TUser> GetById(int id, bool includeDeleted = false);
        Task<TUser> GetByLogin(string login, bool includeDeleted = false);
        Task<IList<TUser>> GetUsersByRole(int roleId, bool includeDeleted = false);
        Task<IList<TUser>> GetUsersByClaim(string claimType, string claimValue, bool includeDeleted = false);
        Task<TUser> GetByEmail(string email, bool includeDeleted = false);
        Task<TUser> Edit(TUser user);
    }
}
