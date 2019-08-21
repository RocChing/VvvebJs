using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VvvebJs.Api
{
    public class ApiHandlerFactory
    {
        private static Dictionary<string, Action<IApplicationBuilder>> dic = new Dictionary<string, Action<IApplicationBuilder>>();


        public static Dictionary<string, Action<IApplicationBuilder>> GetHandlers()
        {
            dic.Add("/save", ApiHandler.Save);
            dic.Add("/list",ApiHandler.List);
            dic.Add("/test", ApiHandler.Test);
            return dic;
        }
    }
}
