using System;
using System.Net;

namespace CoreBase
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpException(HttpStatusCode StatusCode, string Message) : base(Message)
        {
            this.StatusCode = StatusCode;
        }

        public HttpException(string Message) : this(HttpStatusCode.BadRequest, Message)
        { }
    }
}
