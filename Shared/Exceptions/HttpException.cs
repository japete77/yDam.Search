using System;
using System.Net;

namespace yDevs.Shared.Exceptions
{
    public class HttpException : Exception
    {
        private readonly int httpStatusCode;

        private readonly int internalErrorCode;

        public HttpException(string message, int internalErrorCode = -1) : base(message)
        {
            this.httpStatusCode = (int)HttpStatusCode.InternalServerError;
            this.internalErrorCode = internalErrorCode;
        }

        public HttpException(int httpStatusCode, int internalErrorCode = -1)
        {
            this.httpStatusCode = httpStatusCode;
            this.internalErrorCode = internalErrorCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, int internalErrorCode = -1)
        {
            this.httpStatusCode = (int)httpStatusCode;
            this.internalErrorCode = internalErrorCode;
        }

        public HttpException(int httpStatusCode, string message, int internalErrorCode = -1) : base(message)
        {
            this.httpStatusCode = httpStatusCode;
            this.internalErrorCode = internalErrorCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, int internalErrorCode = -1) : base(message)
        {
            this.httpStatusCode = (int)httpStatusCode;
            this.internalErrorCode = internalErrorCode;
        }

        public HttpException(int httpStatusCode, string message, Exception inner, int internalErrorCode = -1) : base(message, inner)
        {
            this.httpStatusCode = httpStatusCode;
            this.internalErrorCode = internalErrorCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner, int internalErrorCode = -1) : base(message, inner)
        {
            this.httpStatusCode = (int)httpStatusCode;
            this.internalErrorCode = internalErrorCode;
        }

        public int StatusCode { get { return this.httpStatusCode; } }
        public int ErrorCode { get { return this.internalErrorCode; } }
    }
}