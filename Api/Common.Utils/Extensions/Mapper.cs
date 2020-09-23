using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils.Extensions
{
    public static class Mapper
    {
        public static TResult MapTo<TResult>(this object source, IMapper mapper)
            where TResult : class
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));

            return mapper.Map<TResult>(source);
        }

        public static PagedList<TResult> MapToPagedList<TResult>(this object source, IMapper mapper)
            where TResult : class
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));

            return mapper.Map<PagedList<TResult>>(source);
        }
    }
}
