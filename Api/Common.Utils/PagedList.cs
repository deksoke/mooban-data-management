using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Utils
{
	public class PagedList<T> : List<T>
	{
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;

        public PagedList()
        {

        }

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			AddRange(items);
		}

		public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count();
			var items = source
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			return new PagedList<T>(items, count, pageNumber, pageSize);
		}

		//public static PagedDTO<List<T>> ToPagedDTO(this PagedList<T> source)
		//{
		//	return new PagedDTO<List<T>>()
		//	{
		//		TotalCount = source.TotalCount,
		//		TotalPages = source.TotalPages,
		//		PageSize = source.PageSize,
		//		CurrentPage = source.CurrentPage,
		//		Data = source
		//	};

		//}

		//public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		//{
		//    var count = source.Count();
		//    var items = await source
		//        .Skip((pageNumber - 1) * pageSize)
		//        .Take(pageSize)
		//        .ToListAsync();

		//    return new PagedList<T>(items, count, pageNumber, pageSize);
		//}
	}

}
