using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace VvvebJs.Api
{
    public static class HttpContextExtensions
    {
        public static string GetValue(this HttpContext context, string key, bool getPost = true)
        {
            string res = string.Empty;
            try
            {
                bool flag = context.Request.Query.TryGetValue(key, out StringValues sv);
                if (flag)
                {
                    res = sv.ToString();
                }
                if (string.IsNullOrEmpty(res))
                {
                    if (getPost)
                    {
                        flag = context.Request.Form.TryGetValue(key, out StringValues psv);
                        if (flag) res = psv.ToString();
                    }
                }
            }
            catch { }
            return res;
        }

        public static async Task ResponseText(this HttpContext context, string text)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync(text, Encoding.UTF8);
        }

        public static string MapPath(this HttpContext context, string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            string[] ps = path.Split('/');

            var env = context.RequestServices.GetService<IHostingEnvironment>();
            List<string> list = new List<string>();
            list.Add(env.ContentRootPath);
            if (ps != null && ps.Length > 0)
            {
                list.AddRange(ps);
            }
            return Path.Combine(list.ToArray()); 
        }
    }
}
