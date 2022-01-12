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
        [Display(Name = "Chờ xác nhận")]
        ChoXacNhan = 0,
        [Display(Name = "Đã nhận")]
        NhanDon,
        [Display(Name = "Đang giao")]
        DangGiao = 1,
        [Display(Name = "Đã giao")]
        DaGiao = 2
    }
    public enum enumPayment
    {
        [Display(Name = "Thanh toán trực tiếp")]
        Offline = 0,
        [Display(Name = "Thanh toán online")]
        Online = 1
    }
}
