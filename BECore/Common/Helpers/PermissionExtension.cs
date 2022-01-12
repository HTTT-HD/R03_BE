using System.Linq;

namespace Common.Helpers
{
    public static class PermissionExtension
    {
        public static bool checkAdmin(this string permisison)
        {
            if(!string.IsNullOrWhiteSpace(permisison) && permisison.Split(',').Any(x => x.ToLower() == Constants.Permission.Admin))
            {
                return true;
            }
            return false;
        }

        public static bool checkAdminNguoiBanHang(this string permisison)
        {
            if (!string.IsNullOrWhiteSpace(permisison) && permisison.Split(',').Any(x => x.ToLower() == Constants.Permission.Admin 
                || x.ToLower() == Constants.Permission.NguoiBanHang))
            {
                return true;
            }
            return false;
        }
        public static bool checkAdminBHGH(this string permisison)
        {
            if (!string.IsNullOrWhiteSpace(permisison) && permisison.Split(',').Any(x => x.ToLower() == Constants.Permission.Admin
                || x.ToLower() == Constants.Permission.NguoiBanHang || x.ToLower() == Constants.Permission.NguoiGiaoHang))
            {
                return true;
            }
            return false;
        }
    }
}
