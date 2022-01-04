using System;

namespace Common.ViewModels.Role
{
    public class RoleViewModel
    {
        public Guid? Id { get; set; }
        public string TenVaiTro { get; set; }
    }

    public class RoleRequest : IPageFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TenVaiTro { get; set; }
    }

}
