using Common.DataAccess.Interfaces;
using Common.Entities;
using Common.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.Repositories
{
    //public interface IUserRoleRepository : IBaseRepository<UserRole>
    //{
    //}

    public interface IUserRoleRepository: IBaseRepository<UserRole>
    {
        Task<int> Add(UserRole userRole);
        Task<UserRole> Get(int userId, int roleId);
        Task<bool> Delete(int userId, int roleId);
        Task<IList<string>> GetByUserId(int userId);
        Task<IList<UserRole>> GetRoleByUserId(int userId);
    }

    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbConnection db): base(db)
        {
        }

        public async Task<int> Add(UserRole userRole)
        {
            return await this.Insert(userRole);
        }

        public async Task<bool> Delete(int userId, int roleId)
        {
            var conditions = new List<Tuple<string, dynamic>>() {
                new Tuple<string, dynamic>("userId = @UserId", new { UserId = userId }),
                new Tuple<string, dynamic>("roleId = @RoleId", new { RoleId = roleId })
            };
            var entities = await this.GetList(conditions);
            foreach(var entity in entities)
            {
                await this.Delete(entity);
            }
            return entities.Count() > 0;
        }

        public async Task<UserRole> Get(int userId, int roleId)
        {
            var conditions = new List<Tuple<string, dynamic>>() {
                new Tuple<string, dynamic>("userId = @UserId", new { UserId = userId }),
                new Tuple<string, dynamic>("roleId = @RoleId", new { RoleId = roleId })
            };
            var entities = await this.GetList(conditions);
            return entities.FirstOrDefault();
        }

        public async Task<IList<string>> GetByUserId(int userId)
        {
            string sql = "SELECT distinct b.* FROM UserRoles as A INNER JOIN Roles as B ON A.RoleId = B.RoleId and a.UserId = @UserId;";
            var entities = await this._db.QueryAsync<Role>(sql, new { UserId = userId });
            return entities.Select(c => c.Name).Distinct().ToList();
        }

        public async Task<IList<Role>> GetRoleByUserId(int userId)
        {
            string sql = "SELECT distinct b.* FROM UserRoles as A INNER JOIN Roles as B ON A.RoleId = B.RoleId and a.UserId = @UserId;";
            var entities = await this._db.QueryAsync<Role>(sql, new { UserId = userId });
            return entities.ToList();
        }

        Task<IList<UserRole>> IUserRoleRepository.GetRoleByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
