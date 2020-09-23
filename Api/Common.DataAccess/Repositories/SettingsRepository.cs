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
    public interface ISettingsRepository : IBaseRepository<Settings>
    {
    }

    public class SettingsRepository : BaseRepository<Settings>, ISettingsRepository
    {
        public SettingsRepository(IDbConnection db): base(db)
        {
        }
    }
}
