using System.Net;

namespace CareerHub.Core.Common.Abstract
{
    public interface IApiResult<T>
    {
        HttpStatusCode HttpStatusCode { get; set; }
        bool IsSuccess { get; set; }
        string Message { get; set; }
        T Data { get; set; }
    }
}
