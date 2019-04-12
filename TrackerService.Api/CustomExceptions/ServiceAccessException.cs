using System;
using System.Net.Http;

namespace TrackerService.Api.CustomExceptions
{
    public class ServiceAccessException : Exception
    {
        public ServiceAccessException(HttpResponseMessage response)
        {
            Response = response;
        }

        public ServiceAccessException(string message, Exception innerException, HttpResponseMessage response) : base(message, innerException)
        {
            Response = response;
        }

        public HttpResponseMessage Response { get; }
    }
}
