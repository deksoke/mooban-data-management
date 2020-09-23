using Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.Interfaces
{
    public interface IPageSearchable<TEntity, TCriteria>
    {
        Task<PagedList<TEntity>> GetPagedList(TCriteria criteria, Pager pager);
    }
}
