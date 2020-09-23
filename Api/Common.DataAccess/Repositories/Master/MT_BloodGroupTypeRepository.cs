using Common.DataAccess.Interfaces;
using Common.Entities.Master;
using Common.Utils;
using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DataAccess.Repositories.Master
{
    public interface IMT_BloodGroupTypeRepository : IBaseRepository<MT_BloodGroupType>, IPageSearchable<MT_BloodGroupType, MT_BloodGroupTypeSearchCriteria>
    {
    }

    public class MT_BloodGroupTypeRepository : BaseEntityRepository<MT_BloodGroupType>, IMT_BloodGroupTypeRepository
    {
        public MT_BloodGroupTypeRepository(IDbConnection db): base(db)
        {
        }

        public async Task<PagedList<MT_BloodGroupType>> GetPagedList(MT_BloodGroupTypeSearchCriteria criteria, Pager pager)
        {
            const string countQuery = @"SELECT COUNT(1)
                                            FROM Peoples l
                                            /**where**/";

            const string selectQuery = @"  SELECT  *
                            FROM    ( SELECT    ROW_NUMBER() OVER ( /**orderby**/ ) AS RowNum, l.*
                                      FROM Peoples l
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
            var rows = await _db.QueryAsync<MT_BloodGroupType>(selector.RawSql, selector.Parameters);
            return new PagedList<MT_BloodGroupType>(rows.ToList(), totalCount, pager.PageNumber, pager.PageSize);
        }
    }

    public class MT_BloodGroupTypeSearchCriteria
    {
        public string IdCard { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
    }
}
