using Common.Helpers; 
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class DanhMuc : BaseModel
    {
        public string TenDanhMuc { get; set; }
        [JsonIgnore]
        public string TenDanhMucKd { get { return TenDanhMuc.ConvertToUnSign(); } set { } }

        public string MoTa { get; set; }
        [JsonIgnore]
        public string MoTaKd { get { return MoTa.ConvertToUnSign(); } set { } }
    }
    
}
