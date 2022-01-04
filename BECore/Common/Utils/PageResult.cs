using Common.Helpers;
using System.Collections.Generic;

namespace Common.Utils
{
    public class PaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int TotalPage { get; set; }
        public PaginationResult<T> Page(IEnumerable<T> items, int pageIndex, int pageSize, long totalCount)
        {
            var totalPage = totalCount.ToTotalPage(pageSize);
            return new PaginationResult<T>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPage = totalPage
            };
        }
    }
}
