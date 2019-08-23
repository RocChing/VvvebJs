using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using VvvebJs.Model;
using Newtonsoft.Json;

namespace VvvebJs.Api
{
    public class ApiHandler
    {
        public static void Save(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.ResponseText(Save(context));
            });
        }

        public static void List(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.ResponseText(List(context));
            });
        }

        public static void Test(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.ResponseText("这是一个测试api");
            });
        }

        private static string List(HttpContext context)
        {
            string path = context.MapPath(AppConst.Pages);
            var files = FileUtils.GetFiles(path);
            List<Page> pages = new List<Page>();
            foreach (var item in files)
            {
                pages.Add(GetPage(item));
            }
            return JsonConvert.SerializeObject(pages);
        }

        private static string Save(HttpContext context)
        {
            try
            {
                string startTemplateUrl = context.GetValue("startTemplateUrl");
                string html = "";
                if (string.IsNullOrEmpty(startTemplateUrl))
                {
                    html = context.GetValue("html");
                    //if (!string.IsNullOrEmpty(html))
                    //{
                    //    int maxLength = 1024 * 1024 * 2;
                    //    html = html.Substring(0, maxLength);
                    //}
                }
                else
                {
                    string url = SanitizeFileName(startTemplateUrl);
                    string path = context.MapPath(url);
                    html = FileUtils.ReadAllText(path);
                }
                if (string.IsNullOrEmpty(html))
                {
                    return "File content is empty";
                }
                string fileName = context.GetValue("fileName");
                fileName = SanitizeFileName(fileName);
                string filePath = context.MapPath(fileName);
                FileUtils.WriteAllText(filePath, html);
                var page = GetPage(filePath);
                return JsonConvert.SerializeObject(page);
            }
            catch (Exception ex)
            {
                return "Error saving file " + ex.Message;
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            string res = Regex.Replace(fileName, @"[^\/\\a-zA-Z0-9\-\._]", "");
            res = Regex.Replace(res, @"\.{2,}", "");
            res = Regex.Replace(res, @"\?.*$", "");
            res = "/" + res.TrimStart('/');
            return res;
        }

        private static Page GetPage(string path)
        {
            string[] ps = path.Split('\\');
            string[] paths = FilterPaths(ps);
            string[] pathWithOutExts = FilterPaths(ps, true);
            string url = string.Join("/", paths);
            var titles = pathWithOutExts.Select(m => $"[{m}]");
            string fileName = paths[paths.Length - 1];

            return new Page()
            {
                Url = url,
                Name = string.Join(".", pathWithOutExts),
                Title = string.Join("", titles),
                Type = GetPageType(path)
            };
        }

        private static string[] FilterPaths(string[] paths, bool flag = false)
        {
            List<string> res = new List<string>();
            var list = paths.Reverse();
            string pages = AppConst.Pages.ToLower();
            int index = 0;
            foreach (var item in list)
            {
                if (item.ToLower() == pages)
                {
                    res.Add(item);
                    break;
                }
                if (flag)
                {
                    if (index == 0)
                    {
                        int len = item.LastIndexOf('.');
                        if (len > -1)
                            res.Add(item.Substring(0, len));
                        else
                            res.Add(item);
                    }
                    else
                    {
                        res.Add(item);
                    }
                }
                else res.Add(item);
                index++;
            }
            res.Reverse();
            return res.ToArray();
        }

        private static string GetPageType(string fileName)
        {
            string type = string.Empty;
            string ext = FileUtils.GetFileExtension(fileName);
            switch (ext)
            {
                case ".cshtml":
                    type = "CsHtml";
                    break;
                case ".vue":
                    type = "Vue";
                    break;
                default:
                    type = "Html";
                    break;
            }
            return type;
        }
    }
}
