using CareerHub.Entities.Common.Abstract;
using System.Text.Json.Serialization;

namespace CareerHub.Entities.Common.Concrete
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        [JsonIgnore]
        public bool IsActve { get; set; } = true;
        [JsonIgnore]
        public bool IsDeleted { get ; set ; }
        [JsonIgnore]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public T CreatedUserId { get; set; }
        [JsonIgnore]
        public DateTime ModifiedTime { get ; set ; } = DateTime.Now;
        [JsonIgnore]
        public T ModifiedUserId { get ; set ; }
        [JsonIgnore]
        public DateTime? DeletedTime { get ; set ; }
        [JsonIgnore]
        public T DeletedUserId { get ; set ; }
    }
}
