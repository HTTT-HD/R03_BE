using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum enumGioiTinh
    {
        [Display(Name = "Khác")]
        Khac = 0,
        [Display(Name = "Nữ")]
        Nu = 1,
        [Display(Name = "Nam")]
        Nam = 2,
    }

}
