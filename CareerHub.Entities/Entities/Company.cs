using CareerHub.Entities.Common.Concrete;

namespace CareerHub.Entities.Entities
{
    public class Company:BaseEntity<int>
    {
        public int CompanyId { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public int RemainingJobPostingRights { get; set; }
    }
}
