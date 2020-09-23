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
    public interface IUserClaimRepository<TUserClaim> where TUserClaim : UserClaim
    {
        Task<IList<TUserClaim>> GetByUserId(int userId);
        Task Delete(int userId, string claimType, string claimValue);
        Task<TUserClaim> Add(TUserClaim userClaim);
        Task<IList<TUserClaim>> EditMany(IList<TUserClaim> userClaims);
        Task<IList<TUserClaim>> GetList(int userId, string claimType, string claimValue);
    }
}
