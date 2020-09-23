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
    public interface IUserRoleRepository<TUserRole> where TUserRole : UserRole
    {
        Task<TUserRole> Add(TUserRole userRole);
        Task<TUserRole> Get(int userId, int roleId);
        Task Delete(int userId, int roleId);
        Task<IList<string>> GetByUserId(int userId);
    }
}
