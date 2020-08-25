using DVG.WIS.Core.Constants;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text;

namespace DVG.WIS.Core
{
    public class UrlHelper
    {
        public static string BuildCategoryUrl(string catName)
        {
            return $"/{Utils.UnicodeToUnsignAndDash(catName)}";
        }

        public static string BuildProductUrl(string catName)
        {
            return $"/san-pham{BuildCategoryUrl(catName)}";
        }

        public static string BuildProductDetailUrl(string name, string cateAlias = "", string url = "", string id = "")
        {
            //return $"/{Utils.UnicodeToUnsignAndDash(name)}-p{id}";

            StringBuilder urlTemp = new StringBuilder();
            string urlDetail = string.Empty;

            if (!string.IsNullOrEmpty(cateAlias))
            {
                urlTemp.Append($"{cateAlias}/");
            }

            if (string.IsNullOrEmpty(url))
            {
                urlTemp.Append($"{Utils.UnicodeToUnsignAndDash(name)}");
            }
            else
            {
                string urlWithoutExtention = url.Replace(".html", "");
                string[] arrStr = urlWithoutExtention.Split('/');
                if (arrStr.Length > 1)
                {
                    int start = urlWithoutExtention.IndexOf(arrStr[arrStr.Length - 1]);
                    int end = urlWithoutExtention.Length - start;
                    url = urlWithoutExtention.Substring(start, end);
                }
                else
                {
                    url = urlWithoutExtention;
                }

                urlTemp.Append($"{url.TrimStart('/')}");
            }

            if (!string.IsNullOrEmpty(id))
            {
                urlTemp.Append($"-{id}");
            }

            urlDetail = urlTemp.ToString().Replace(".html", "").TrimStart('/');

            return urlDetail;
        }

        public static string BuildArticleDetailUrl(string name, string cateAlias = "", string url = "", string id = "")
        {
            StringBuilder urlTemp = new StringBuilder();
            string urlDetail = string.Empty;

            if (!string.IsNullOrEmpty(cateAlias))
            {
                urlTemp.Append($"{cateAlias}/");
            }

            if (string.IsNullOrEmpty(url))
            {
                urlTemp.Append($"{Utils.UnicodeToUnsignAndDash(name)}");
            }
            else
            {
                string urlWithoutExtention = url.Replace(".html", "");
                string[] arrStr = urlWithoutExtention.Split('/');
                if (arrStr.Length > 1)
                {
                    int start = urlWithoutExtention.IndexOf(arrStr[arrStr.Length - 1]);
                    int end = urlWithoutExtention.Length - start;
                    url = urlWithoutExtention.Substring(start, end);
                }
                else
                {
                    url = urlWithoutExtention;
                }

                urlTemp.Append($"{url.TrimStart('/')}");
            }

            if (!string.IsNullOrEmpty(id))
            {
                urlTemp.Append($"-{id}");
            }

            urlDetail = string.Format("{0}{1}", urlTemp.ToString().Replace(".html", ""), ".html").TrimStart('/');

            return urlDetail;
        }

        public static string BuildAbsoluteUrl(string relativeUrl, string domain = "")
        {
            if (string.IsNullOrEmpty(relativeUrl) || relativeUrl.StartsWith("http"))
                return relativeUrl;
            return string.Format("{0}/{1}", string.IsNullOrEmpty(domain) ? StaticVariable.Domain.TrimEnd('/') : domain.TrimEnd('/'), relativeUrl.TrimStart('/'));
        }

        public static int HasRedirect(string newUrl, HttpContext context)
        {
            var statusCode = StatusCodes.Status200OK;
            if (context != null)
            {
                statusCode = StatusCodes.Status301MovedPermanently;

                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.Headers[HeaderNames.Location] = newUrl;
                context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            }
            return statusCode;
        }
    }
}