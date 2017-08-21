using System;
using System.Web;
using System.Web.Mvc;

namespace LoveMM.Filters
{
    public class AuthorizeHttpModules : IHttpModule
    {
        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpRequest request = application.Request;

            string requestType = request.HttpMethod.ToLower();
            string path = request.RawUrl.ToString().ToLower();
            string requestPath = path.Split('?')[0].Split('/')[1].ToLower();
            string[] needLoginPath = new string[] { "admin/login" };
            bool needLogin = false;
            if (string.IsNullOrEmpty(requestPath))
            {
                needLogin = true;
            }
            else
            {
                foreach (string item in needLoginPath)
                {
                    if (requestPath == item)
                    {
                        needLogin = true;
                        break;
                    }
                }
            }

            if (needLogin)
            {
                if (context.Session["AdminInfo"] == null)//未登陆
                {
                    if (new HttpRequestWrapper(request).IsAjaxRequest())
                    {
                        context.Response.StatusCode = 1;
                        context.Response.End();
                    }
                    else
                    {
                        string url = "/Admin/Login";
                        context.Response.Redirect(url, true);
                    }
                }
            }
        }
    }
}