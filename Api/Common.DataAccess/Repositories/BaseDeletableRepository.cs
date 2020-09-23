using Common.DataAccess.Interfaces;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Linq;

namespace Common.DataAccess.Repositories
{
    public abstract class BaseDeletableRepository<TEntity>: BaseEntityRepository<TEntity>, IBaseRepository<TEntity>
          where TEntity : DeletableEntity, new()
    {
        public BaseDeletableRepository(IDbConnection db): base(db)
        {
            _db = db;
        }

        public virtual async Task<IEnumerable<TEntity>> GetList(List<Tuple<string, dynamic>> conditions, bool includeDeleted = false)
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
            builder.Where("IsDeleted = @IsDeleted", new { @IsDeleted = includeDeleted ? 0 : 1 });

            foreach (var p in conditions)
            {
                builder.Where(p.Item1, p.Item2);
            }

            return await _db.QueryAsync<TEntity>(selector.RawSql, selector.Parameters);
        }

        public override async Task<int> Insert(TEntity entity)
        {
            entity.IsDeleted = false;
            return await base.Insert(entity);
        }
        public override async Task<bool> Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeleteDate = DateTime.Now;
            return await base.Delete(entity);
        }

        public async Task<bool> Exist(TEntity entity, bool includeDeleted = false)
        {
            // var condition = new Tuple<string, dynamic>("", new { @p = "" });
            var rows = await this.GetList(null, includeDeleted);
            return rows.Count() > 0;
        }
    }
}
