using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System;
using System.Text.Json.Serialization;

namespace Domain.Models
{

    public interface IBaseModel
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid? UpdateBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BaseModel : IBaseModel
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [JsonIgnore]
        public DateTime CreateAt { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public Guid CreateBy { get; set; }
        [JsonIgnore]
        public DateTime? UpdateAt { get; set; }
        [BsonRepresentation(BsonType.String)]
        [JsonIgnore]
        public Guid? UpdateBy { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
    }
}
