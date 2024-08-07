using System.Text.Json.Serialization;

namespace CareerHub.Core.Parameters
{
    public class CompanyRequestModel
    {
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public int RemainingJobPostingRights { get; set; }
    }
}
