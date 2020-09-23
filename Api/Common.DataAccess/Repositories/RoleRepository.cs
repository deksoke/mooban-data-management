using Common.DataAccess.Interfaces;
using Common.Entities;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> Get(string name);
        Task<Role> FindByNameAsync(string name);
    }

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnection db): base(db)
        {
        }

        public async Task<Role> FindByNameAsync(string name)
        {
            var conditions = new List<Tuple<string, dynamic>>() {
                new Tuple<string, dynamic>("Name = @Name", new { Name = name })
            };
            var rows = await this.GetList(conditions);
            return rows.FirstOrDefault();
        }

        public async Task<Role> Get(string name)
        {
            var conditions = new List<Tuple<string, dynamic>>() {
                new Tuple<string, dynamic>("Name = @Name", new { Name = name })
            };
            var rows = await this.GetList(conditions);
            return rows.FirstOrDefault();
        }
    }
}
