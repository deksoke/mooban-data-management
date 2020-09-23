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
    public abstract class BaseRepository<TEntity>: IBaseRepository<TEntity>
       where TEntity : class
    {
        protected IDbConnection _db;

        public BaseRepository(IDbConnection db)
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

        public virtual async Task<IEnumerable<TEntity>> GetList(List<Tuple<string, dynamic>> conditions)
        {
            const string selectQuery = @"SELECT *
                                        FROM @TableName l
                                        /**where**/
                                        ORDER BY Id";

            SqlBuilder builder = new SqlBuilder();
            var tableName = typeof(TEntity).Name;

            var selector = builder.AddTemplate(selectQuery, new
            {
                TableName = tableName
            });

            foreach (var p in conditions)
            {
                builder.Where(p.Item1, p.Item2);
            }

            return await _db.QueryAsync<TEntity>(selector.RawSql, selector.Parameters);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _db.GetAsync<TEntity>(id);
        }

        public virtual Task<int> Insert(TEntity entity)
        {
            return _db.InsertAsync(entity);
        }

        public virtual Task<bool> Update(TEntity entity)
        {
            return _db.UpdateAsync(entity);
        }

        public virtual Task<bool> Delete(TEntity entity)
        {
            return _db.DeleteAsync(entity);
        }
    }

}
