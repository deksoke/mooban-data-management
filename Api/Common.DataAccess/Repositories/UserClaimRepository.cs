using Common.DataAccess.Interfaces;
using Common.Entities;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.Repositories
{
    public interface IUserClaimRepository : IBaseRepository<UserClaim>
    {
        Task<bool> Delete(int userId, string claimType, string claimValue);
        Task<IList<UserClaim>> GetByUserId(int userId);
    }

    public class UserClaimRepository : BaseRepository<UserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(IDbConnection db): base(db)
        {
        }

        public Task<bool> Delete(int userId, string claimType, string claimValue)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserClaim>> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
