using System;
using Microsoft.AspNetCore.Mvc.Filters;
using yDevs.Services.Logger;

namespace yDevs.Shared.Logging
{
    public class LoggingFilter : IActionFilter
    {
        private readonly ILoggerService _loggerService;
        public LoggingFilter(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["Request_Start"] = DateTime.UtcNow;            
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            DateTime requestStart = (DateTime)context.HttpContext.Items["Request_Start"];
            TimeSpan elapsedTime = DateTime.UtcNow - requestStart;
            
            _loggerService.Logger().Debug("{LogType:l} {Action:l} on controller {Controller:l} executed by {User:l} user in {ElapsedTime} ms",
                "Action",
                context.ActionDescriptor.RouteValues["action"],
                context.ActionDescriptor.RouteValues["controller"],
                this.GetUsername(context),
                elapsedTime.TotalMilliseconds
                );
        }

        private string GetUsername(ActionExecutedContext context)
        {
            // TODO: Extract username from JWT
            // Http Request Header (Authorization: Bearer <token>)
            return "unkown";            
        }
    }
}