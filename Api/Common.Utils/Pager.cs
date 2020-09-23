using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils
{
    public class Pager
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string Sort { get; set; }

        public List<SortDescriptor> GetSortings()
        {
            List<SortDescriptor> sortings = new List<SortDescriptor>();

            if (!string.IsNullOrEmpty(Sort))
            {
                foreach (var c in Sort.Split(","))
                {
                    var sorter = new SortDescriptor();
                    if (c.StartsWith("-"))
                    {
                        sorter.Direction = SortDescriptor.SortingDirection.Descending;
                        sorter.Field = c.Trim().Substring(1);
                    }
                    else
                    {
                        sorter.Direction = SortDescriptor.SortingDirection.Ascending;
                        sorter.Field = c.Trim();
                    }
                    sortings.Add(sorter);
                }
            }

            return sortings;
        }
    }

}
