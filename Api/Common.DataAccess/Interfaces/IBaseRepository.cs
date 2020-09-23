using Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllPaged(Pager pager);
        Task<int> Insert(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }

    public interface IBaseEntityRepository<TEntity>: IBaseRepository<TEntity> where TEntity : class
    {
        Task<bool> Exist(TEntity entity);
    }
}
