using System;
using System.Net;

namespace TrackerService.Api.CustomExceptions
{
    public class UserManagementException : ApplicationException
    {
        public UserManagementException(HttpStatusCode status, string content, Exception ex) : base(null, ex)
        {
            Status = status;
            Content = content;
        }

        public HttpStatusCode Status { get; }
        public string Content { get; }
    }
}
