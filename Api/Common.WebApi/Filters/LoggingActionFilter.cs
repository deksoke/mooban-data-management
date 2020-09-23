using Common.WebApi.ConnectionsDb;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.WebApi.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly IDiagnosticContext _diagnosticContext;
        public LoggingActionFilter(IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string requestId = "";
            var headers = context.HttpContext.Request.Headers["RequestId"];
            if (headers.Count > 0)
            {
                requestId = headers[0];
            }

            _diagnosticContext.Set("RequestId", requestId);
            _diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            _diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
            _diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
            _diagnosticContext.Set("ValidationState", context.ModelState.IsValid);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var headers = context.HttpContext.Request.Headers["RequestId"];
            if (headers.Count > 0)
            {
                string requestId = headers[0];
                context.HttpContext.Response.Headers.Add("RequestId", requestId);
            }

        }
    }
}
