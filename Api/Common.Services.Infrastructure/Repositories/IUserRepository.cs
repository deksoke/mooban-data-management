/*
* Copyright (c) Akveo 2019. All Rights Reserved.
* Licensed under the Single Application / Multi Application License.
* See LICENSE_SINGLE_APP / LICENSE_MULTI_APP in the ‘docs’ folder for license information on type of purchased license.
*/

using Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Services.Infrastructure.Repositories
{
    public interface IUserRepository<TUser> where TUser : User
    {
        Task Delete(int id);
        Task<TUser> GetByLogin(string login, bool includeDeleted = false);
        Task<TUser> GetByEmail(string email, bool includeDeleted = false);
        Task<TUser> Get(int id, bool includeDeleted = false);
        Task<TUser> Edit(TUser user);
    }
}
