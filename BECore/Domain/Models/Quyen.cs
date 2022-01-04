using Common.Helpers;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Quyen : BaseModel
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        [JsonIgnore]
        public string TenKd { get { return Ten.ConvertToUnSign(); } set { } }
    }
}
