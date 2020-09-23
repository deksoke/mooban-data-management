using Common.DataAccess.Interfaces;
using Common.Entities.Master;
using Common.Utils;
using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DataAccess.Repositories.Standard
{
    public interface ISTD_PeopleRepository : IBaseRepository<STD_People>, IPageSearchable<STD_People, PeopleSearchCriteria>
    {
    }

    public class STD_PeopleRepository : BaseDeletableRepository<STD_People>, ISTD_PeopleRepository
    {
        public STD_PeopleRepository(IDbConnection db): base(db)
        {
        }

        public async Task<PagedList<STD_People>> GetPagedList(PeopleSearchCriteria criteria, Pager pager)
        {
            const string countQuery = @"SELECT COUNT(1)
                                            FROM STD_Peoples l
                                            /**where**/";

            const string selectQuery = @"  SELECT  *
                            FROM    ( SELECT    ROW_NUMBER() OVER ( /**orderby**/ ) AS RowNum, l.*
                                      FROM STD_Peoples l
                                      /**where**/
                                    ) AS RowConstrainedResult
                            WHERE   RowNum > ((@PageIndex - 1) * @PageSize)
                                AND RowNum <= (@PageIndex * @PageSize)
                            ORDER BY RowNum";

            SqlBuilder builder = new SqlBuilder();

            var count = builder.AddTemplate(countQuery);
            var selector = builder.AddTemplate(selectQuery, new { PageIndex = pager.PageNumber, PageSize = pager.PageSize });

            if (!string.IsNullOrEmpty(criteria.IdCard))
            {
                var msg = "%" + criteria.IdCard + "%";
                builder.Where("l.IdCard = @Message", new { Message = criteria.IdCard });
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
            var rows = await _db.QueryAsync<STD_People>(selector.RawSql, selector.Parameters);
            return new PagedList<STD_People>(rows.ToList(), totalCount, pager.PageNumber, pager.PageSize);
        }
    }

    public class PeopleSearchCriteria
    {
        public string IdCard { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
    }
}
