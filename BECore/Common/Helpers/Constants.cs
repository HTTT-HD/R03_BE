
namespace Common.Helpers
{
    public static class Constants
    {
        public static class ElasticSearch
        {
            public const string URL = "http://localhost:9200";
            public const bool Enabled = true;
            public const string Enviroment = "dev";
            public const string UserEs = "users";
            public const string Products = "products";
        }

        public static class AuthConfig
        {
            public const string CheckPass = nameof(CheckPass);
            public const string HashSalt = nameof(HashSalt);
            public const string MinLength = nameof(MinLength);
            public const string MongoConnection = "MongodbConnectionStrings:";
            public const string HangfireDb = "HangfireDb";
            public const string Database = "DatabaseName";
            public const string ServerName = "ServerName";
            public const string FolderFileDefault = "documents";
            public const string SaltConfig = "Y5w6DlvKJCSsIl%Gp*4gBtgpMnNY987s8AX&jt3Sb1LSpT21KS%ApSf7bJk*3f!EPrHNn&KQoaop8zoeL@4s*Edrx9^z4aryEHL";

        }

        public static class MessageResponse
        {
            public const string DuplicateName = "Tên đã bị trùng. Vui lòng đổi tên khác!";
            public const string DuplicateCode = "Mã đã bị trùng. Vui lòng đổi mã khác!";
            public const string NotFound = "Bản ghi đã chọn không tồn tại!";
            public const string UploadImageFailed = "Tải ảnh lên thất bại.";
            public const string WrongImageType = "Loại ảnh truyền lên không đúng.";
            public const string ConvertToListFailed = "Danh sách truyền vào không hợp lệ.";
            public const string HasChildItem = "Có đơn vị con không thể xóa.";
            public const string SSOFailed = "SSO không chính xác";
            public const string NotValue = "Không có dữ liệu";
            public const string NotStatus = "Trạng thái không hợp lệ";
            public const string ConflictUser = "Tên đăng nhập của thành viên đã tồn tại";
            public const string ConflictPermission = "Quyền đã tồn tại!";
            public const string ConflictUserInRole = "Tài khoản đã tồn tại!";
            public const string LoginFailed = "Tài khoản hoặc mật khẩu không chính xác!";
            public const string LoginSuccessfully = "Đăng nhập thành công!";
            public const string NotEnough = "Số lượng sản phẩm không đủ!";

        }
        public static class CodeError
        {
            public const string Duplicate = "G001";
            public const string NotFound = "G002";
            public const string UploadFailed = "G003";
            public const string WrongType = "G004";
            public const string HasChildItem = "G005";
            public const string NotValue = "G006";
            public const string NotStatus = "G007";
            public const string Conflict = "G008";
            public const string NotEnough = "G008";
        }

        public static class Principal
        {
            public const string Permission = "Permission";
            public const string HoTen = "HoTen";
            public const string UserId = "UserId";
            public const string SoDienThoai = "SoDienThoai";
        }

        public static class Permission
        {
            public const string Admin = "admin";
            public const string NguoiBanHang = "ban-hang";
            public const string NguoiGiaoHang = "giao-hang";
        }
    }
}
