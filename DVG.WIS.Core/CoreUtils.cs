using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using System.Collections.Generic;
using DVG.WIS.Core.Constants;
using System.ComponentModel;

namespace DVG.WIS.Core
{
    public static class CoreUtils
    {
        const string defaultFormatDate = "dd/MM/yyyy";
        const string fullFormatDate = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// DungDT2 : Convert string to datetime type
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="format">Format for date return</param>
        /// <returns>Datetime type</returns>
        public static DateTime ConvertStringToDateTime(string value, string format = defaultFormatDate)
        {
            if (string.IsNullOrEmpty(value)) return DateTime.MinValue;

            string strRegex = @"\d{2}/\d{2}/\d{4}\s\d{2}:\d{2}:\d{2}";
            Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match myMatch = myRegex.Match(value);
            if (myMatch.Success)
            {
                format = fullFormatDate;
            }
            else
            {
                format = defaultFormatDate;
            }

            return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DungDT2 : Convert string to datetime type
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="format">Format for date return</param>
        /// <returns>Datetime type</returns>
        public static DateTime? ConvertStringToDateTimeNullable(string value, string format = defaultFormatDate)
        {
            if (string.IsNullOrEmpty(value)) return DateTime.MinValue;

            string strRegex = @"\d{2}/\d{2}/\d{4}\s\d{2}:\d{2}:\d{2}";
            Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match myMatch = myRegex.Match(value);
            if (myMatch.Success)
            {
                format = fullFormatDate;
            }
            else
            {
                format = defaultFormatDate;
            }

            DateTime parseOut;
            if (DateTime.TryParseExact(value, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out parseOut))
            {
                return parseOut;
            }
            return null;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static long CreateNewsId()
        {
            const string formatDate = "yyyyMMddHHmmssFFF";

            var dt = DateTime.Now;

            var dateToString = dt.ToString(formatDate);

            while (dateToString.Length < formatDate.Length)
            {
                dateToString += "0";
            }

            return Int64.Parse(dateToString);
        }

        public static string BuildAvatar(string avatar, string defaultAvatar = "")
        {
            if (string.IsNullOrEmpty(avatar)) return !string.IsNullOrEmpty(defaultAvatar) ? defaultAvatar : StaticVariable.NoImage;

            var domain = StaticVariable.DomainImage.TrimEnd('/');
            return avatar.Contains(domain) ? avatar : domain + "/" + avatar.TrimStart('/');
        }

        public static string BuildCropAvatar(string avatar, string crop = "", string defaultAvatar = "")
        {
            if (string.IsNullOrEmpty(avatar)) return !string.IsNullOrEmpty(defaultAvatar) ? defaultAvatar : StaticVariable.NoImage;

            var domain = StaticVariable.DomainImage.TrimEnd('/');
            avatar = avatar.Contains(domain) ? avatar.Replace(domain, "") : avatar;

            return domain + "/" + crop.Trim('/') + "/" + avatar.TrimStart('/');
        }

        public static string AddAttributeForAnchors(string htmlContent, string domainTarget = "http://banxehoi.com/diendan/seolink/?refer=", bool isEncrypt = false)
        {
            if (String.IsNullOrEmpty(htmlContent)) return htmlContent;
            try
            {
                htmlContent = Regex.Replace(htmlContent, @"rel=[""']nofollow[""']", String.Empty);
                htmlContent = htmlContent.Replace(@"target=[""']_blank[""']", String.Empty);

                string strRegex = @"(?<LINK><a[^>]href=[""'](?<url>[^""']+)[""'](?<attrs>[^>]*)>(?<Content>((?!<\/a>).)*)<\/a>)";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match match = myRegex.Match(htmlContent);
                //string strReplace = @"<a href=""" + domainTarget + @"${url}"" ${attrs} rel=""nofollow"" target=""_blank"">${Content}</a>";

                if (match.Success)
                {
                    htmlContent = myRegex.Replace(htmlContent, delegate (Match m)
                    {
                        string url = domainTarget + m.Groups["url"].Value;
                        string attrs = m.Groups["attrs"].Value;
                        string content = m.Groups["Content"].Value;
                        string link = string.Format(@"<a href=""{0}"" {1} rel=""nofollow"" target=""_blank"">{2}</a>", url, attrs,
                            content);
                        link = Regex.Replace(link, @"\s+", " ");
                        return link;
                    });
                    //htmlContent = myRegex.Replace(htmlContent, strReplace);
                }

            }
            catch
            {
                // Todo something
            }
            return htmlContent;
        }

        public static string ExtractImageFromcontent(string content)
        {
            string firstImage = String.Empty;

            if (String.IsNullOrEmpty(content)) return firstImage;

            try
            {

                string strRegex = @"<img.+?src=[\""'](?<SRC>.+?)[\""'].*?>";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                foreach (Match matchAvatar in myRegex.Matches(content))
                {
                    if (matchAvatar.Success)
                    {
                        firstImage = matchAvatar.Groups["SRC"].Value;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Todo
            }
            return firstImage;
        }

        public static string UploadImageIncontent(string content, out string firstImage, Func<string, string> uploadImage)
        {
            firstImage = String.Empty;

            string newContent = content;

            if (String.IsNullOrEmpty(newContent)) return newContent;

            try
            {

                string strRegex = @"<img.+?src=[\""'](?<SRC>.+?)[\""'].*?>";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                Match match = myRegex.Match(newContent);
                if (match.Success)
                {
                    newContent = myRegex.Replace(newContent, m => string.Format("<p style=\"text-align:center\"><img src=\"{0}\" /></p>", uploadImage.Invoke(m.Groups["SRC"].Value)));
                }


                foreach (Match matchAvatar in myRegex.Matches(newContent))
                {
                    if (matchAvatar.Success)
                    {
                        firstImage = matchAvatar.Groups["SRC"].Value;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Todo
            }
            return newContent;
        }

        /// <summary>
        /// Use the Excel Formula Coach to figure out a monthly loan payment. At the same time, you'll learn how to use the PMT function in a formula.
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="nper"></param>
        /// <param name="pv"></param>
        /// <param name="fv"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double PMT(double rate, double nper, double pv, double fv = 0, double type = 0)
        {
            return rate * (fv + pv * Math.Pow(1 + rate, nper)) / ((Math.Pow(1 + rate, nper) - 1) * (1 + rate * type));
        }

        /// <summary>
        /// Build URL động, cho phép tự do truyền format và arguments khác nhau.
        /// <para>&#160;</para>
        /// Example: BuildURL("/{0}/{1}-n{2}/", new object[] { "tin-tuc", "tieu-de-tin-khong-dau", 20160718203512456 });&#160;
        /// <para>&#160;</para>
        /// ==> /tin-tuc/tieu-de-tin-khong-dau-n20160718203512456
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string BuildURL(string format, params object[] args)
        {
            if (String.IsNullOrEmpty(format)) return String.Empty;


            string outputUrl = format;

            if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
            {
                outputUrl = string.Format(outputUrl, args);
            }

            if (!outputUrl.StartsWith("http"))
            {
                if (!outputUrl.StartsWith("/"))
                {
                    outputUrl = string.Concat("/", outputUrl);
                }
            }

            return outputUrl;
        }

        public static string BuildURLForTag(string format, string tagUnsignName, int tagId, int pageIndex = 0)
        {
            StringBuilder output = new StringBuilder();
            if (tagId > 0 && !string.IsNullOrEmpty(tagUnsignName))
            {
                if (pageIndex > 0)
                {
                    string filter = string.Format("tag{0}/p{1}", tagId, pageIndex);
                    output.AppendFormat(format, tagUnsignName, filter);
                }
                else
                {
                    string filter = string.Format("tag{0}", tagId);
                    output.AppendFormat(format, tagUnsignName, filter);
                }

            }
            return output.ToString();
        }

        ///// <summary>
        ///// Build link for news detail
        ///// </summary>
        ///// <param name="cateUrl"></param>
        ///// <param name="newsTitle"></param>
        ///// <param name="newsUrl"></param>
        ///// <param name="newsId"></param>
        ///// <returns></returns>
        //public static string BuildURLForNews(string newsTitle, string newsUrl, int newsType, long newsId)
        //{
        //    if (string.IsNullOrEmpty(newsUrl) || newsUrl.Length > 150)
        //    {
        //        if (!string.IsNullOrEmpty(newsTitle))
        //        {
        //            string initTitle = "";
        //            if (newsTitle.Length > 150)
        //            {
        //                initTitle = newsTitle.Substring(0, 150);
        //                newsUrl = StringUtils.UnicodeToUnsignCharAndDash(initTitle);
        //            }
        //            else
        //            {
        //                newsUrl = StringUtils.UnicodeToUnsignCharAndDash(newsTitle);
        //            }
        //        }
        //        else newsUrl = Guid.NewGuid().ToString();
        //    }

        //    string formatUrl = ConstUrl.NewsDetailUrl;

        //    switch (newsType)
        //    {
        //        case (int)NewsTypeEnum.Advices:
        //            formatUrl = ConstUrl.AdvicesDetailUrl;
        //            break;
        //        case (int)NewsTypeEnum.Gallery:
        //            formatUrl = ConstUrl.GalleryDetailUrl;
        //            break;
        //        //case (int)NewsTypeEnum.Assessment:
        //        //    formatUrl = ConstUrl.AssessmentDetailUrl;
        //        //    break;
        //        case (int)NewsTypeEnum.Pricing:
        //            formatUrl = ConstUrl.PricingDetailUrl;
        //            break;
        //        case (int)NewsTypeEnum.News:
        //        default:
        //            formatUrl = ConstUrl.NewsDetailUrl;
        //            break;
        //    }
        //    return BuildURL(formatUrl, new object[] { newsUrl, newsId });
        //}

        public static string BuildURLForAMP(string formatUrl, string standardUrl)
        {
            return BuildURL(formatUrl, standardUrl );
        }

        /// <summary>
        /// Build URL động, cho phép tự do truyền format và arguments khác nhau.
        /// Example: BuildURLForRouterConfig("/diendan/{0}/{1}-tid{2}/", "topic", "ten-thread-khong-dau", 15);
        /// Dùng trong RouterConfig
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string BuildURLForRouterConfig(string format, params object[] args)
        {
            if (String.IsNullOrEmpty(format)) return String.Empty;

            string outputUrl = format.TrimStart('/');

            if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
            {
                outputUrl = string.Format(outputUrl, args);
            }

            return outputUrl;
        }


        public static string AddMeta(string tagName, string name, string value, Dictionary<string, string> attributeds = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value)) return string.Empty;

            string valueName = "content";
            if (name.ToLower().Contains("robots"))
                valueName = "rel";
            string result = string.Empty;
            if (attributeds != null)
            {
                result = string.Format("<{0} name=\"{1}\" {2}=\"{3}\" />", tagName, name, valueName, value);
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            else
            {
                result = string.Format("<meta name=\"{0}\" {1}=\"{2}\" />", name, valueName, value);
            }
            return result;
        }

        public static string AddMeta(string name, string value, Dictionary<string, string> attributeds = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value)) return string.Empty;

            string valueName = "content";
            if (name.ToLower().Contains("robots"))
                valueName = "rel";
            string result = string.Empty;
            if (attributeds != null)
            {
                result = string.Format("<meta name=\"{0}\" {1}=\"{2}\" ", name, valueName, value);
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            else
            {
                result = string.Format("<meta name=\"{0}\" {1}=\"{2}\" />", name, valueName, value);
            }
            return result;
        }

        public static string AddMeta(string tagName, Dictionary<string, string> attributeds)
        {
            string result = string.Empty;
            if (attributeds != null)
            {
                result = string.Concat("<", tagName, " ");
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            return result;
        }

        public static string AddMeta(Dictionary<string, string> attributeds)
        {
            string result = string.Empty;
            if (attributeds != null)
            {
                result = "<meta";
                foreach (KeyValuePair<string, string> key in attributeds)
                {
                    result = string.Concat(result, string.Format(" {0}=\"{1}\" ", key.Key, key.Value));
                }
                result = string.Concat(result, " />");
            }
            return result;
        }

        public static string ConvertNumberToCurrency(double number, string sperator = ".")
        {
            if (number <= 0)
            {
                return "0";
            }

            number = Math.Round(number, 0);

            string output = StringUtils.FormatNumber(number.ToString(CultureInfo.CurrentCulture), sperator);

            return output;
        }

        //public static string GenarateCanonicalUrl(string standardUrl)
        //{
        //    bool isMobile = DetectDevice.Instance.BrowserIsMobile();

        //    string hostRequest = HttpContext.Current.Request.Url.Host.ToLower();

        //    hostRequest = hostRequest.Replace(string.Format(":{0}", HttpContext.Current.Request.Url.Port), String.Empty);

        //    string hostConfig = StaticVariable.Domain;
        //    string mobileHostConfig = StaticVariable.DomainMobile;

        //    string baseUrl = StaticVariable.BaseUrl;
        //    string mobileBaseUrl = StaticVariable.BaseMobileUrl;

        //    if (hostConfig != mobileHostConfig)
        //    {
        //        if (hostRequest.Equals(hostConfig))
        //        {
        //            isMobile = false;
        //        }
        //        else if (hostRequest.Equals(mobileHostConfig))
        //        {
        //            isMobile = true;
        //        }
        //    }
        //    if (isMobile)
        //    {
        //        standardUrl = baseUrl + standardUrl.Replace(mobileHostConfig, String.Empty);
        //    }
        //    else
        //    {
        //        standardUrl = mobileBaseUrl + standardUrl.Replace(hostConfig, String.Empty);
        //    }
        //    return standardUrl;
        //}

        public static string GenarateAlternateUrl(string standardUrl)
        {
            string hostConfig = StaticVariable.Domain;
            string mobileBaseUrl = StaticVariable.BaseMobileUrl;
            standardUrl = mobileBaseUrl + standardUrl.Replace(hostConfig, String.Empty);
            return standardUrl;
        }

        public static string GenarateTitle(string objectTitle, bool isShowSurfix = true)
        {
            if (string.IsNullOrEmpty(objectTitle)) return objectTitle;

            objectTitle = Utilities.StringUtils.RemoveStrHtmlTags(objectTitle);

            //if (isShowSurfix)
            //{
            //    return string.Format("{0} - {1}", objectTitle, AppSettings.Instance.GetString(Const.HostName));
            //}
            return objectTitle;
        }

        public static string GenPagingAjax(string functionName, int pageIndex, int pageSize, int totalCount)
        {
            int pageNum = (int)Math.Ceiling((double)totalCount / pageSize);
            if (pageNum * pageSize < totalCount)
            {
                pageNum++;
            }
            string htmlPage = String.Empty;
            const string buildlink = "<li><a href=\"javascript:{0}('{1}')\" class=\"{2}\" title=\"{4}\">{3}</a></li>";
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            string currentPage = pageIndex.ToString(); // trang tiện tại
            int iCurrentPage = 0;
            if (pageIndex <= 0) iCurrentPage = 1;
            else iCurrentPage = pageIndex;
            string active;

            if (pageNum <= 7)
            {
                if (pageNum != 1)
                {
                    for (int i = 1; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        htmlPage += string.Format(buildlink, functionName, i, active, i, "Trang " + i);
                    }
                }
            }
            else
            {
                if (iCurrentPage < (pageNum - 4))
                {
                    if (iCurrentPage <= 3)
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            htmlPage += string.Format(buildlink, functionName, i, active, i, "Trang " + i);
                        }
                    }
                    else
                    {
                        htmlPage += string.Format(buildlink, functionName, (iCurrentPage - 1), String.Empty, "Trang trước", "Trang trước");
                        for (int i = iCurrentPage - 2; i <= iCurrentPage + 2; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            htmlPage += string.Format(buildlink, functionName, i, active, i, "Trang " + i);
                        }
                    }
                    htmlPage += "<li>&rarr;</li>";
                    htmlPage += string.Format(buildlink, functionName, (pageNum - 1), String.Empty, (pageNum - 1), "Trang " + (pageNum - 1));
                    htmlPage += string.Format(buildlink, functionName, pageNum, String.Empty, pageNum, "Trang " + pageNum);
                    htmlPage += string.Format(buildlink, functionName, (iCurrentPage + 1), String.Empty, "Trang sau", "Trang sau");
                }
                else
                {
                    htmlPage += string.Format(buildlink, functionName, (iCurrentPage - 1), String.Empty, "Trang trước", "Trang trước");
                    htmlPage += string.Format(buildlink, functionName, "2", String.Empty, "2", "2");
                    htmlPage += "<li>&rarr;</li>";
                    for (int i = pageNum - 4; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        htmlPage += string.Format(buildlink, functionName, i, active, i, "Trang " + i);

                    }
                }

            }
            //htmlPage = string.Format("<div class=\"padding\"><ul>{0}</ul></div>", htmlPage);
            return htmlPage;
        }

        public static string NaviPaging(int pageIndex, int pageSize, int totalRow, string href, string fParam = "/p")
        {
            StringBuilder htmlContent = new StringBuilder();
            var numPage = totalRow / pageSize;
            const int pagePerSeg = 10;
            if (numPage == 1 && totalRow > pageSize) numPage++;
            if (numPage > 1)
            {
                var constDelta = Math.Floor(decimal.Parse((pagePerSeg / 2).ToString()));
                var startIndex = pageIndex - constDelta;
                if (startIndex <= 0) startIndex = 1;
                var endIndex = startIndex + pagePerSeg;
                if (endIndex > numPage)
                    endIndex = numPage + 1;

                //if (pageSize > 0) param += "&num=" + pageSize;
                var param = fParam != "" ? fParam : "";

                if (pageIndex == 1)
                {
                    htmlContent.Append("<li><a><i class=\"fa fa-angle-double-left\"></i></a></li>");
                }
                else
                {
                    int previus = pageIndex - 1;
                    if (previus > 1)
                    {
                        htmlContent.AppendFormat("<li><a title=\"Previous\" href=\"{0}{1}{2}\"><i class=\"fa fa-angle-double-left\"></i></a></li>", href, param, (pageIndex - 1));
                    }
                    else
                    {
                        htmlContent.AppendFormat("<li><a title=\"Previous\" href=\"{0}\"><i class=\"fa fa-angle-double-left\"></i></a></li>", href);
                    }
                    //htmlContent.Append("<li><a>...</a></li>");
                }

                for (var index = startIndex; index < endIndex; index++)
                {
                    if (index == pageIndex)
                        htmlContent.Append("<li><a class=\"active\">" + index + "</a></li>");
                    else
                    {
                        if (index == 1)
                        {
                            htmlContent.AppendFormat("<li><a title=\"Xem trang {0}\" href=\"{0}\">1</a></li>", href);
                        }
                        else
                        {
                            htmlContent.AppendFormat("<li><a title=\"Xem trang {0}\" href=\"{1}{2}{0}\">{0}</a></li>", index, href, param);
                        }
                    }
                }

                if (pageIndex >= numPage)
                {
                    htmlContent.Append("<li><a><i class=\"fa fa-angle-double-right\"></i></a></li>");
                }
                else
                {
                    if (numPage > pagePerSeg)
                    {
                        htmlContent.Append("<li><a>...</a></li>");
                    }
                    htmlContent.AppendFormat("<li><a title=\"Next\" href=\"{0}{1}{2}\"><i class=\"fa fa-angle-double-right\"></i></a></li>", href, param, (pageIndex + 1));
                }
            }
            return htmlContent.ToString();
        }

        public static string NaviPagingMobile(int pageIndex, int pageSize, int totalRow, string href, string fParam = "/p")
        {
            StringBuilder htmlContent = new StringBuilder();
            var numPage = totalRow / pageSize;
            const int pagePerSeg = 4;
            if (numPage == 1 && totalRow > pageSize) numPage++;
            if (numPage > 1)
            {
                var constDelta = Math.Floor(decimal.Parse((pagePerSeg / 2).ToString()));
                var startIndex = pageIndex - constDelta;
                if (startIndex <= 0) startIndex = 1;
                var endIndex = startIndex + pagePerSeg;
                if (endIndex > numPage)
                    endIndex = numPage + 1;

                //if (pageSize > 0) param += "&num=" + pageSize;
                var param = fParam != "" ? fParam : "";

                if (pageIndex == 1)
                {
                    // htmlContent.Append("<li><a><i class=\"fa fa-angle-double-left\"></i></a></li>");
                    //htmlContent.Append("<li><a><i class=\"fa fa-angle-left\"></i></a></li>");
                }
                else
                {
                    int previus = pageIndex - 1;
                    if (previus > 1)
                    {
                        htmlContent.AppendFormat("<li><a title=\"Previous\" href=\"{0}{1}{2}\"><i class=\"fa fa-angle-double-left\"></i></a></li>", href, param, 1);

                        htmlContent.AppendFormat("<li><a title=\"Previous\" href=\"{0}{1}{2}\"><i class=\"fa fa-angle-left\"></i></a></li>", href, param, (pageIndex - 1));
                    }
                    else
                    {
                        htmlContent.AppendFormat("<li><a title=\"Previous\" href=\"{0}\"><i class=\"fa fa-angle-double-left\"></i></a></li>", href);
                        htmlContent.AppendFormat("<li><a title=\"Previous\" href=\"{0}\"><i class=\"fa fa-angle-left\"></i></a></li>", href);
                    }
                    //htmlContent.Append("<li><a>...</a></li>");
                }

                for (var index = startIndex; index < endIndex; index++)
                {
                    if (index == pageIndex)
                        htmlContent.Append("<li class=\"active\"><a>" + index + "</a></li>");
                    else
                    {
                        if (index == 1)
                        {
                            htmlContent.AppendFormat("<li><a title=\"Xem trang {0}\" href=\"{0}\">1</a></li>", href);
                        }
                        else
                        {
                            htmlContent.AppendFormat("<li><a title=\"Xem trang {0}\" href=\"{1}{2}{0}\">{0}</a></li>", index, href, param);
                        }
                    }
                }

                if (pageIndex >= numPage)
                {
                    //htmlContent.Append("<li><a><i class=\"fa fa-angle-right\"></i></a></li>");
                    //htmlContent.Append("<li><a title=\"Trang cuối\"><i class=\"fa fa-angle-double-right\"></i></a></li>");
                }
                else
                {
                    //if (numPage > pagePerSeg)
                    //{
                    //    htmlContent.Append("<li><a>...</a></li>");
                    //}

                    htmlContent.AppendFormat("<li><a title=\"Trang" + (pageIndex + 1) + "\" href=\"{0}{1}{2}\"><i class=\"fa fa-angle-right\"></i></a></li>", href, param, (pageIndex + 1));

                    htmlContent.AppendFormat("<li><a title=\"Trang cuối\" href=\"{0}{1}{2}\"><i class=\"fa fa-angle-double-right\"></i></a></li>", href, param, numPage);

                }
            }
            return htmlContent.ToString();
        }

        /// <summary>
        /// Check thời gian delay trước khi cho phép thực hiện tiếp tính từ thời điểm ghi nhận session đầu tiên cho tới hết delayInSeconds so với hiện tại.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="delayInSeconds"></param>
        /// <returns></returns>
        //public static bool DelayBeforeContinue(string key, int delayInSeconds = 5)
        //{
        //    var context = HttpContext.Current;
        //    if (context == null) return false;
        //    DateTime now = DateTime.Now;
        //    if (context.Session[key] != null)
        //    {
        //        DateTime sessionDateTime;
        //        if (DateTime.TryParse(context.Session[key].ToString(), out sessionDateTime))
        //        {
        //            if (sessionDateTime.AddSeconds(delayInSeconds) < now)
        //            {
        //                context.Session.Remove(key);
        //                return false;
        //            }
        //            else return true;
        //        }
        //    }
        //    else
        //    {
        //        context.Session.Add(key, now);
        //    }
        //    return false;
        //}

        /// <summary>
        /// Thay đổi text style cho 
        /// </summary>
        /// <param name="type">1: Chữ hoa - uppercase, 2: lowercase, 3: Capital, 4: chỉ In hoa chữ đầu trong một câu.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TextTransform(int type, string value)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;
            value = value.Trim();
            switch (type)
            {
                case 1:
                    return value.ToUpper();
                case 2:
                    return value.ToLower();
                case 3:
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                case 4:
                    if (value.Length == 1) return value.ToUpper();
                    value = value.ToLower();
                    string valueTemp = value.Substring(0, 1).ToUpper();
                    string valueTemp2 = value.Substring(1, value.Length - 1);
                    return valueTemp + valueTemp2;
                default:
                    return value;
            }
        }

        public static string GetEnumActionType(int value)
        {
            if (value <= 0) return String.Empty;

            if (value == (int)ActivityTypeEnum.Login)
            {
                return Const.ActivityLogin;
            }

            if (value == (int)ActivityTypeEnum.Logout)
            {
                return Const.ActivityLogOut;
            }

            if (value == (int)ActivityTypeEnum.ChangePassword)
            {
                return Const.ActivityChangePass;
            }

            if (value == (int)ActivityTypeEnum.ChangePermission)
            {
                return Const.ActivityAccessPermission;
            }

            if (value == (int)ActivityTypeEnum.AddBank || value == (int)ActivityTypeEnum.EditBank || value == (int)ActivityTypeEnum.DeleteBank)
            {
                return Const.ActivityBank;
            }

            if (value == (int)ActivityTypeEnum.AddLoanPackage || value == (int)ActivityTypeEnum.EditLoanPackage || value == (int)ActivityTypeEnum.DeleteLoanPackage)
            {
                return Const.ActivityLoan;
            }

            if (value == (int)ActivityTypeEnum.AddSavePackage || value == (int)ActivityTypeEnum.EditSavePackage || value == (int)ActivityTypeEnum.DeleteSavePackage)
            {
                return Const.ActivitySaving;
            }

            if (value == (int)ActivityTypeEnum.AddDebitCard || value == (int)ActivityTypeEnum.EditDebitCard || value == (int)ActivityTypeEnum.DeleteDebitCard)
            {
                return Const.ActivityAtmCard;
            }

            if (value == (int)ActivityTypeEnum.AddCreditCard || value == (int)ActivityTypeEnum.EditCreditCard || value == (int)ActivityTypeEnum.DeleteCreditCard)
            {
                return Const.ActivityMaster;
            }

            if (value == (int)ActivityTypeEnum.AddNews || value == (int)ActivityTypeEnum.EditNews || value == (int)ActivityTypeEnum.DeleteNews || value == (int)ActivityTypeEnum.ApprovedNews || value == (int)ActivityTypeEnum.DownNews || value == (int)ActivityTypeEnum.RemoveNews)
            {
                return Const.ActivityNews;
            }

            return string.Empty;

        }

        public static int[] GetRandom(int number, int max)
        {
            int[] lst = new int[4];
            for (int i = 0; i < number; i++)
            {
                lst[i] = -1;
            }
            int x;
            int j = 0;
            Random random = new Random();
            x = random.Next(0, max - 1);
            while (j < number)
            {
                for (int i = 0; i < number; i++)
                {
                    while (x == lst[i])
                    {
                        x = random.Next(number, max);
                        i = 0;
                    }
                }
                lst[j] = x;
                j++;
                x = random.Next(0, max - 1);
            }

            return lst;
        }

        /// <summary>
        /// Hàm này dùng để check cho phép show hay ko các button Đăng ký trên gói vay, thẻ và tiết kiệm
        /// </summary>
        /// <returns></returns>
        public static bool IsShowRegisterButton(int provinceId, int bankId, bool isSavingPackage = false)
        {
            string bankKey = isSavingPackage ? Const.SavingTechcomAndVpBankId : Const.TechcomAndVpBankId;
            string lstProvinceRegs = AppSettings.Instance.GetString(Const.HnAndHcmCityId);
            string lstBankRegs = AppSettings.Instance.GetString(bankKey);

            if (string.IsNullOrEmpty(lstProvinceRegs) && string.IsNullOrEmpty(lstBankRegs)) return true;

            if (!string.IsNullOrEmpty(lstProvinceRegs) && string.IsNullOrEmpty(lstBankRegs))
            {
                if (lstProvinceRegs.Contains("," + provinceId + ","))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(lstProvinceRegs) && !string.IsNullOrEmpty(lstBankRegs))
            {
                if (lstBankRegs.Contains("," + bankId + ","))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(lstProvinceRegs) && !string.IsNullOrEmpty(lstBankRegs))
            {

                // Nếu là tỉnh khác (khác HN & TP.HCM)
                if (!lstProvinceRegs.Contains("," + provinceId + ","))
                {
                    // Nếu NH có trong config thì return true
                    if (lstBankRegs.Contains("," + bankId + ","))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }// Nếu là HN/TP.HCM
                else
                {
                    return true;
                }

            }

            return false;
        }

        public static string ConvertDenominationsCurrency(double value) //chuyển đổi mệnh giá tiền
        {
            string str = "";
            if (value >= 1000000000)
            {
                str = Math.Round(value / 1000000000) + " tỷ ";
            }
            else if (value >= 1000000)
            {
                str = Math.Round(value / 1000000) + " triệu ";
            }
            else
            {
                str = Math.Round(value / 1000) + " nghìn ";
            }
            return str;
        }

        public static string GetTitleSortAndSearchProduct(int type, int orderBycolumn, int orderByType)
        {
            string strSort = "";
            string strOrderBycolumn = "";
            if (type == 1)
            {
                switch (orderBycolumn)
                {
                    case 1:
                        strOrderBycolumn = " lãi suất ";
                        break;
                    case 2:
                        strOrderBycolumn = " thời gian cố định lãi ";
                        break;
                    case 3:
                        strOrderBycolumn = " thanh toán đầu kỳ ";
                        break;
                    case 4:
                        strOrderBycolumn = " tổng lãi phải trả ";
                        break;
                    default:
                        strOrderBycolumn = "";
                        break;
                }
            }
            else if (type == 0)
            {
                switch (orderBycolumn)
                {
                    case 1:
                        strOrderBycolumn = " lãi suất ";
                        break;
                    case 2:
                        strOrderBycolumn = " hạn mức tối đa ";
                        break;
                    case 3:
                        strOrderBycolumn = " phí thường niên ";
                        break;
                    default:
                        strOrderBycolumn = "";
                        break;
                }
            }
            string strOrderByType = "";
            switch (orderByType)
            {
                case 0:
                    strOrderByType = " tăng dần ";
                    break;
                case 1:
                    strOrderByType = " giảm dần ";
                    break;
                default:
                    strOrderByType = "";
                    break;
            }
            if (!string.IsNullOrEmpty(strOrderBycolumn))
            {
                strSort = strOrderBycolumn + strOrderByType;
            }
            return strSort;
        }

        public static string BuildTextSearch(long id, string title, string sapo = "", string content = "")
        {
            string sapoNotHtml = Utilities.StringUtils.RemoveStrHtmlTags(sapo); //Xóa bỏ html
            string contentNotHtml = Utilities.StringUtils.RemoveStrHtmlTags(content); //Xóa bỏ html
            return string.Format("{0}-{1}-{2}-{3}", id, title, sapoNotHtml, contentNotHtml); //Cộng chuỗi lại
        }

        /// <summary>
        /// The get value from description
        /// Author: ThanhDT
        /// Created date: 8/11/2020 12:37 AM
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            return default(T);
        }
    }
}
