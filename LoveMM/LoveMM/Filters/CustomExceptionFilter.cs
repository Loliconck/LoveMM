using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace LoveMM.Filters
{
    public class CustomExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Response==null)
            {
                context.Response = new HttpResponseMessage();
            }
            context.Response.StatusCode = HttpStatusCode.NotImplemented;
            context.Response.Content = new StringContent("出错了");
            base.OnException(context);
        }
    }
}