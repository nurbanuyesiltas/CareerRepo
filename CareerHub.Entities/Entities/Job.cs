using CareerHub.Entities.Common.Concrete;

namespace CareerHub.Entities.Entities
{
    public class Job:BaseEntity<int>
    {
        public int JobId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDuration { get; set; }
        public int QualityScore { get; set; }
        public string? Benefits { get; set; }
        public string? WorkingType { get; set; }
        public string? Salary { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
