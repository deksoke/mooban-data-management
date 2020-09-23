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
    public interface IUserPhotoRepository : IBaseRepository<UserPhoto>
    {
    }

    public class UserPhotoRepository : BaseRepository<UserPhoto>, IUserPhotoRepository
    {
        public UserPhotoRepository(IDbConnection db): base(db)
        {
        }
    }
}
