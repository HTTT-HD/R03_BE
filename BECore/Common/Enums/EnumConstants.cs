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

    public enum enumStatus
    {
        [Display(Name = "Huỷ đơn")]
        HuyDon = -1,
        [Display(Name = "Nhận đơn")]
        NhanDon = 0,
        [Display(Name = "Đang giao")]
        DangGiao = 1,
        [Display(Name = "Đã giao")]
        DaGiao = 2
    }
}
