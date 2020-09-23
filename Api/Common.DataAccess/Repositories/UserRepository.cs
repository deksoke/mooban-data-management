using Common.DataAccess.Interfaces;
using Common.Entities;
using Common.Utils;
using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DataAccess.Repositories
{
    public interface IUserRepository : IBaseRepository<User>, IPageSearchable<User, UserSearchCriteria>
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByLoginAsync(string loginName);
        Task<User> FindByIdAsync(int id);
    }

    public class UserRepository : BaseDeletableRepository<User>, IUserRepository
    {
        public UserRepository(IDbConnection db): base(db)
        {
        }

        public async Task<PagedList<User>> GetPagedList(UserSearchCriteria criteria, Pager pager)
        {
            const string countQuery = @"SELECT COUNT(1)
                                            FROM Users l
                                            /**where**/";

            const string selectQuery = @"  SELECT  *
                            FROM    ( SELECT    ROW_NUMBER() OVER ( /**orderby**/ ) AS RowNum, l.*
                                      FROM Users l
                                      /**where**/
                                    ) AS RowConstrainedResult
                            WHERE   RowNum > ((@PageIndex - 1) * @PageSize)
                                AND RowNum <= (@PageIndex * @PageSize)
                            ORDER BY RowNum";

            SqlBuilder builder = new SqlBuilder();

            var count = builder.AddTemplate(countQuery);
            var selector = builder.AddTemplate(selectQuery, new { PageIndex = pager.PageNumber, PageSize = pager.PageSize });

            if (!string.IsNullOrEmpty(criteria.Id))
            {
                var msg = "%" + criteria.Id + "%";
                builder.Where("l.Id = @Message", new { Message = criteria.Id });
            }
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                var msg = "%" + criteria.Name + "%";
                builder.Where("(l.FirstName like @Message or l.LastName like @Message)", new { Message = msg });
            }

            var sortings = pager.GetSortings();
            if (sortings != null)
            {
                foreach (var sorting in sortings)
                {
                    if (string.IsNullOrWhiteSpace(sorting.Field))
                        continue;

                    if (sorting.Direction == SortDescriptor.SortingDirection.Ascending)
                    {
                        builder.OrderBy(sorting.Field);
                    }
                    else if (sorting.Direction == SortDescriptor.SortingDirection.Descending)
                    {
                        builder.OrderBy(sorting.Field + " desc");
                    }
                }
            }

            var totalCount = (await _db.QueryAsync<int>(count.RawSql, count.Parameters)).Single();
            var rows = await _db.QueryAsync<User>(selector.RawSql, selector.Parameters);
            return new PagedList<User>(rows.ToList(), totalCount, pager.PageNumber, pager.PageSize);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            const string selectQuery = @"SELECT * FROM Users l where l.Email = @Email";

            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(selectQuery, new { Email = email });

            var rows = await _db.QueryAsync<User>(selector.RawSql, selector.Parameters);
            return rows.FirstOrDefault();
        }

        public async Task<User> FindByLoginAsync(string loginName)
        {
            const string selectQuery = @"SELECT * FROM Users l where l.Login = @Login";

            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(selectQuery, new { Login = loginName });

            var rows = await _db.QueryAsync<User>(selector.RawSql, selector.Parameters);
            return rows.FirstOrDefault();
        }

        public async Task<User> FindAsync(string loginName, string passwordHash)
        {
            const string selectQuery = @"SELECT * FROM Users l where l.Login = @Login and l.PasswordHash = @PasswordHash";

            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(selectQuery, new { Login = loginName, PasswordHash = passwordHash });

            var rows = await _db.QueryAsync<User>(selector.RawSql, selector.Parameters);
            return rows.FirstOrDefault();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            const string selectQuery = @"SELECT * FROM Users l where l.Id = @Id";

            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(selectQuery, new { Id = id });

            var rows = await _db.QueryAsync<User>(selector.RawSql, selector.Parameters);
            return rows.FirstOrDefault();
        }
    }

    public class UserSearchCriteria
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
