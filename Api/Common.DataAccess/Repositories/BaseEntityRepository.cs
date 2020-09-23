using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Common.DataAccess.Interfaces;
using Common.Entities;
using Common.Utils;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Common.DataAccess.Repositories
{
    public abstract class BaseEntityRepository<TEntity>: IBaseEntityRepository<TEntity>
       where TEntity : BaseEntity
    {
        protected internal IDbConnection _db;

        public BaseEntityRepository(IDbConnection db)
        {
            _db = db;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _db.GetAllAsync<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllPaged(Pager pager)
        {
            var sql = "SELECT * FROM @TableName ORDER BY Id Limit @Limit Offset @Offset";
            var tableName = typeof(TEntity).Name;
            int offset = (pager.PageSize * (pager.PageNumber - 1));
            int limit = pager.PageSize;

            return await _db.QueryAsync<TEntity>(sql, new
            {
                TableName = tableName,
                Limit = limit,
                Offset = offset
            });
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _db.GetAsync<TEntity>(id);
        }

        public virtual async Task<int> Insert(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            return await _db.InsertAsync(entity);
        }

        //
        // Returns:
        //     true if updated, false if not found or not modified (tracked entities)
        //
        public virtual async Task<bool> Update(TEntity entity)
        {
            entity.UpdateDate = DateTime.Now;
            return await _db.UpdateAsync(entity);
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            return await _db.DeleteAsync(entity);
        }

        public virtual async Task<bool> Exist(TEntity entity)
        {
            return (await this.GetById(entity.Id)) == null;
        }
    }

}
