using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace KazApi.Common._Filter
{
    /// <summary>
    /// 例外フィルター
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            
            response.ContentType = "application/json";
            response.StatusCode = context.Exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
            
            var errorResponse = new 
            { 
                message = context.Exception.Message,
                exceptionType = context.Exception.GetType().Name 
            };
            
            context.Result = new JsonResult(errorResponse);
        }
    }
}
