using Common.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Vaitro : BaseModel
    {
        public string TenVaiTro { get; set; }
        [JsonIgnore]
        public string TenKd { get { return TenVaiTro.ConvertToUnSign(); } set { } }

        [BsonRepresentation(BsonType.String)]
        [JsonIgnore]
        public List<Guid> ThanhViens { get; set; }
        [BsonRepresentation(BsonType.String)]
        [JsonIgnore]
        public List<Guid> Quyen { get; set; }
    }
}
