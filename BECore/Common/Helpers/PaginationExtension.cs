using System;

namespace Common.Helpers
{
    public static class PaginationExtension
    {
        public static int ToTotalPage(this long totalCount, int pageSize)
        {
            return (int)Math.Ceiling(totalCount / (double)pageSize);
        }
        public static int ToTotalPage(this int totalCount, int pageSize)
        {
            return (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
