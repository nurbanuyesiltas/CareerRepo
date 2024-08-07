namespace CareerHub.Entities.Common.Abstract
{
    public interface IBaseEntity<T>
    {
        bool IsActve { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreatedTime { get; set; }
        T CreatedUserId { get; set; }
        DateTime ModifiedTime { get; set; }
        T ModifiedUserId { get; set; }
        DateTime? DeletedTime { get; set; }
        T DeletedUserId { get; set; }
    }
}
