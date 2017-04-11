using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using yDevs.Services.Logger;

namespace yDevs.Shared.Exceptions
{
   internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _loggerService;

        public HttpExceptionMiddleware(RequestDelegate next, ILoggerService loggerService)
        {
            this._next = next;
            this._loggerService = loggerService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                this.LogException(httpException);

                context.Response.StatusCode = httpException.StatusCode;
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = httpException.Message;

                await context.Response.WriteAsync(new ErrorDescription
                {
                    ErrorCode = httpException.ErrorCode,
                    Message = httpException.Message,
                }.ToString(), Encoding.UTF8);
            }
            catch (TimeoutException timeoutException)
            {
                this.LogException(timeoutException);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new ErrorDescription
                {
                    ErrorCode = timeoutException.HResult,
                    Message = "Error connecting to database. Please check the database server."

                }.ToString(), Encoding.UTF8);
            }
            catch (Exception unhandledException)
            {
                this.LogException(unhandledException, true);
                                
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new ErrorDescription
                {
                    ErrorCode = -1,
                    Message = "Unhandled error occurred. Please review internal logs for more details."

                }.ToString(), Encoding.UTF8);
            }
        }

        private void LogException(Exception exception, bool generic = false)
        {
            try
            {
                //"{LogType:l} occured:\n{ExceptionMessage}\nInnerException:{InnerException}\nStack: {StackTrace}"
                this._loggerService.Logger().Error("{LogType:l} occured: {ExceptionMessage:l}",
                    generic ? "Exception" : exception.GetType().Name,
                    exception.Message, 
                    exception.InnerException != null ? exception.InnerException.Message : "", 
                    exception.StackTrace);
            }
            catch
            {                    
            }
        }
    }
}