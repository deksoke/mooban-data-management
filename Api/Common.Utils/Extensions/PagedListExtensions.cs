using AutoMapper;
using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils.Extensions
{
    public static class PagedListExtensions
    {
        public static PagedDTO<List<R>> ToPagedDTO<T, R>(this PagedList<T> source, IMapper mapper)
        {
            return new PagedDTO<List<R>>()
            {
                TotalCount = source.TotalCount,
                TotalPages = source.TotalPages,
                PageSize = source.PageSize,
                CurrentPage = source.CurrentPage,
                Data = source.MapTo<List<R>>(mapper)
            };
        }
    }
}
