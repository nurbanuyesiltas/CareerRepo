using CareerHub.Core.Common.Abstract;
using System.Net;
using System.Text.Json.Serialization;

namespace CareerHub.Core.Common.Concrete
{
    public class ApiResult<T> : IApiResult<T>
    {
        [JsonIgnore]
        public HttpStatusCode HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

}
