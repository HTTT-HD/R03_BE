using System;

namespace Common.ViewModels.Permission
{
    public class PermissionRequest
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public Guid? VaiTroId { get; set; }
    }

    public class PermissionResponse
    {
        public Guid Id { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public bool SuDung { get; set; }
    }

    public class AddPermisisonToRole
    {
        public Guid VaiTroId { get; set; }
        public Guid QuyenId { get; set; }
        public bool Them { get; set; }
    }

    public class AddUserToRole
    {
        public Guid VaiTroId { get; set; }
        public Guid ThanhVienId { get; set; }
        public bool Them { get; set; }
    }
}
