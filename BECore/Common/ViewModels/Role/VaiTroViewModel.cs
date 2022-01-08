using System;

namespace Common.ViewModels.Role
{
    public class RoleViewModel
    {
        public Guid? Id { get; set; }
        public string TenVaiTro { get; set; }
    }

    public class RoleRequest : PageFilter
    {
        public string TenVaiTro { get; set; }
    }

}
