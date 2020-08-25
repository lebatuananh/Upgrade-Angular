using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Web;
using DVG.WIS.Utilities.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace DVG.WIS.Utilities
{
    public static class Utils
    {
        public static string DateFomat(DateTime dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" <label class=\"date\">29</label>", dt.Day);
            switch (dt.Month)
            {
                case 1:
                    sb.AppendFormat("<label class=\"month\">Th một</label>");
                    break;
                case 2:
                    sb.AppendFormat("<label class=\"month\">Th hai</label>");
                    break;
                case 3:
                    sb.AppendFormat("<label class=\"month\">Th ba</label>");
                    break;
                case 4:
                    sb.AppendFormat("<label class=\"month\">Th tư</label>");
                    break;
                case 5:
                    sb.AppendFormat("<label class=\"month\">Th năm</label>");
                    break;
                case 6:
                    sb.AppendFormat("<label class=\"month\">Th sáu</label>");
                    break;
                case 7:
                    sb.AppendFormat("<label class=\"month\">Th bảy</label>");
                    break;
                case 8:
                    sb.AppendFormat("<label class=\"month\">Th tám</label>");
                    break;
                case 9:
                    sb.AppendFormat("<label class=\"month\">Th chín</label>");
                    break;
                case 10:
                    sb.AppendFormat("<label class=\"month\">Th mười</label>");
                    break;
                case 11:
                    sb.AppendFormat("<label class=\"month\">Th mười một</label>");
                    break;
                default:
                    sb.AppendFormat("<label class=\"month\">Th mười hai</label>");
                    break;
            }

            return sb.ToString();
        }
        public static string SubstringWithdot(string input, int length)
        {
            try
            {
                if (input.Length > length)
                    return input.Substring(0, length) + "...";
                else return input;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return string.Empty;
        }

        public static int GetNumber(string input)
        {
            try
            {
                return Convert.ToInt32(Regex.Replace(input, "\\D", string.Empty));
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Chuyển danh sách từ xâu sang mảng int.
        /// Các mảng int có định dạng được ngăn cách nhau bằng dấu ,
        /// VD: 1,2,3,4
        /// </summary>
        /// <param name="LtsSourceValues">Xâu chứa mảng int</param>
        /// <returns>Mảng int</returns>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static List<int> String2ListInt(string LtsSourceValues)
        {
            List<int> LtsValues = new List<int>();
            try
            {
                if (!string.IsNullOrEmpty(LtsSourceValues))
                {
                    if (LtsSourceValues.Contains(","))
                        LtsValues = LtsSourceValues.Trim(',').Split(',').Select(o => Convert.ToInt32(o)).ToList();
                    else
                        LtsValues.Add(Convert.ToInt32(LtsSourceValues));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return LtsValues;
        }

        /// <summary>
        /// Đọc 1 file trên file vật lý
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
        public static string PagerPublish(string Controler, int CatId, int RowPerPage, int CurrentPage, int totalrecod)
        {
            StringBuilder sb = new StringBuilder();
            int totalpage = totalrecod % RowPerPage == 0 ? totalrecod / RowPerPage : totalrecod / RowPerPage + 1;
            sb.Append("<ul class=\"page-numbers\">");
            //sb.AppendFormat("<a class=\" btn_small ui-state-default ui-corner-all tooltip\" title=\"First Page\" {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, 1, Keyword, SearchIn));

            sb.AppendFormat("<li><a href=\"#\">Prev</a></li>");

            //sb.Append("<span class=\"ui-icon ui-icon-arrowthickstop-1-w\"></span>");
            //sb.Append("</a>");
            //sb.AppendFormat("<a class=\" btn_small ui-state-default ui-corner-all tooltip\" title=\"Previous Page\"  {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage - 1 > 0 ? CurrentPage - 1 : 1, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-circle-arrow-w\"></span>");
            //sb.Append("</a>");
            //sb.AppendFormat("<select class=\"pagesize\" {0}>", GoPageChange(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, "this.value", Keyword, SearchIn));
            for (int i = 1; i <= totalpage; i++)
            {
                if (i == CurrentPage)
                {
                    sb.AppendFormat("<li><a href=\"{0}\" class=\"current\">{1}</a></li>", "#", i);
                }
                else
                {
                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", "#", i);
                }
            }
            //sb.Append(" </select>");
            //sb.AppendFormat("<a class=\"btn_small ui-state-default ui-corner-all tooltip\" title=\"Next Page\" {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage + 1 <= totalpage ? CurrentPage + 1 : totalpage, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-circle-arrow-e\"></span>");
            //sb.Append("</a>");
            //sb.AppendFormat("<a class=\" btn_small ui-state-default ui-corner-all tooltip\" title=\"Last Page\" {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, totalpage, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-arrowthickstop-1-e\"></span>");
            //sb.Append("</a>");
            sb.AppendFormat("<li><a href=\"#\">Next</a></li>");
            sb.Append("</ul>");





            return sb.ToString();
        }
        /// <summary>
        /// Hàm phân trang
        /// </summary>
        /// <param name="Controler"></param>
        /// <param name="CatId"></param>
        /// <param name="Status"></param>
        /// <param name="FieldSort"></param>
        /// <param name="FieldOption"></param>
        /// <param name="RowPerPage"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="Keyword"></param>
        /// <param name="SearchIn"></param>
        /// <param name="totalrecod"></param>
        /// <returns></returns>
        public static string Pager(string Controler, int CatId, int Status, string FieldSort, bool FieldOption, int RowPerPage, int CurrentPage, string Keyword, string SearchIn, int totalrecod)
        {
            StringBuilder sb = new StringBuilder();
            int totalpage = totalrecod % RowPerPage == 0 ? totalrecod / RowPerPage : totalrecod / RowPerPage + 1;
            //sb.Append("<div class=\"float-right\">");
            //sb.Append("<div id=\"pager\">");
            //sb.AppendFormat("<a class=\" btn_small ui-state-default ui-corner-all tooltip\" title=\"First Page\" {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, 1, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-arrowthickstop-1-w\"></span>");
            //sb.Append("</a>");
            //sb.AppendFormat("<a class=\" btn_small ui-state-default ui-corner-all tooltip\" title=\"Previous Page\"  {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage - 1 > 0 ? CurrentPage - 1 : 1, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-circle-arrow-w\"></span>");
            //sb.Append("</a>");
            //sb.AppendFormat("<select class=\"pagesize\" {0}>", GoPageChange(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, "this.value", Keyword, SearchIn));
            //for (int j = 1; j <= totalpage; j++)
            //{
            //    if (j == CurrentPage)
            //    {
            //        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">Page {0}</option>", j);
            //    }
            //    else
            //    {
            //        sb.AppendFormat("<option value=\"{0}\" >Page {0}</option>", j);
            //    }
            //}
            //sb.Append(" </select>");
            //sb.AppendFormat("<a class=\"btn_small ui-state-default ui-corner-all tooltip\" title=\"Next Page\" {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage + 1 <= totalpage ? CurrentPage + 1 : totalpage, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-circle-arrow-e\"></span>");
            //sb.Append("</a>");
            //sb.AppendFormat("<a class=\" btn_small ui-state-default ui-corner-all tooltip\" title=\"Last Page\" {0}>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, totalpage, Keyword, SearchIn));
            //sb.Append("<span class=\"ui-icon ui-icon-arrowthickstop-1-e\"></span>");
            //sb.Append("</a>");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("<div class=\"datatable-footer\">");
            sb.Append("<div class=\"dataTables_paginate paging_full_numbers\" id=\"data-table_paginate\">");

            if (CurrentPage > 1)
            {
                sb.AppendFormat("<a class=\"first paginate_button \" tabindex=\"0\" id=\"data-table_first\" {0}>First</a>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, 1, Keyword, SearchIn));
                sb.AppendFormat("<a class=\"previous paginate_button \" tabindex=\"0\" id=\"data-table_previous\" {0}>&lt;</a>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage - 1, Keyword, SearchIn));
            }
            else
            {
                sb.AppendFormat("<a class=\"first paginate_button paginate_button_disabled\" tabindex=\"0\" id=\"data-table_first\" {0}>First</a>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, 0, Keyword, SearchIn));
                sb.Append("<a class=\"previous paginate_button paginate_button_disabled\" tabindex=\"0\" id=\"data-table_previous\">&lt;</a>");
            }
            sb.Append(" <span>");
            sb.AppendFormat("<select class=\"\" name=\"select\" {0}>", GoPageChange(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, "this.value", Keyword, SearchIn));
            for (int j = 1; j <= totalpage; j++)
                if (j == CurrentPage)
                    sb.AppendFormat("<option value=\"{0}\" selected>Trang {0}</option>", j);
                else
                    sb.AppendFormat("<option value=\"{0}\">Trang {0}</option>", j); ;
            sb.Append("</select>");
            sb.Append("</span>");
            if (CurrentPage < totalpage)
            {
                sb.AppendFormat("<a class=\"next paginate_button\" tabindex=\"0\" id=\"data-table_next\" {0}>&gt;</a>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage + 1, Keyword, SearchIn));
                sb.AppendFormat("<a class=\"last paginate_button\" tabindex=\"0\" id=\"data-table_last\" {0}>Last</a></div></div>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, totalpage, Keyword, SearchIn));
            }
            else
            {
                sb.AppendFormat("<a class=\"next paginate_button paginate_button_disabled\" tabindex=\"0\" id=\"data-table_next\" {0}>&gt;</a>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage + 1, Keyword, SearchIn));
                sb.AppendFormat("<a class=\"last paginate_button paginate_button_disabled\" tabindex=\"0\" id=\"data-table_last\" {0}>Last</a></div></div>", GoPage(Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, totalpage, Keyword, SearchIn));
            }
            return sb.ToString();
        }

        private static string GoPage(string Controler, int CatId, int Status, string FieldSort, bool FieldOption, int RowPerPage, int CurrentPage, string Keyword, string SearchIn)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("onclick=\"GoPage('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}');\"", Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage, Keyword, SearchIn);
            return sb.ToString();
        }

        private static string GoPageChange(string Controler, int CatId, int Status, string FieldSort, bool FieldOption, int RowPerPage, string CurrentPage, string Keyword, string SearchIn)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("onchange=\"GoPage('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}');\"", Controler, CatId, Status, FieldSort, FieldOption, RowPerPage, CurrentPage, Keyword, SearchIn);
            return sb.ToString();
        }

        public static string DropdowlistSearch(int tabindex, string[] Field, string selected)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<select tabindex=\"3\" class=\"field text small\" name=\"ddlSearch\" id=\"ddlSearch\">");
            foreach (var item in Field)
            {
                if (item.Equals(selected))
                    sb.AppendFormat("<option value=\"{0}\" selected>{1}</option>", item, item);
                else
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item, item);
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển từ kích thước file sang định dạng chuẩn bao gồm:
        /// Bytes, Kb, MB, GB
        /// </summary>
        /// <param name="bytes">Chiều dài mảng Byte[]. Thường là size.Length</param>
        /// <returns>Xâu đã format</returns>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static string FormatBytes(int bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        /// <summary>
        /// Hàm tính toán tỷ trọng % dựa trên số lượng và tổng
        /// </summary>
        /// <param name="currentValue">Giá trị thực tế</param>
        /// <param name="totalValue">Tổng giá trị</param>
        /// <returns>Tỷ lệ % giữa giá trị thực tế và tổng giá trị</returns>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static string GetDataPercent(int currentValue, int totalValue)
        {
            try
            {
                double value = ((double)currentValue / totalValue);
                return (value.ToString("0.0%") == "NaN") ? "0" : value.ToString("0.0%");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Hàm chuyển một chuỗi tiếng việt có dấu thành tiếng việt không dấu
        /// </summary>
        /// <param name="Unicode">xâu tiếng việt có dấu</param>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static string UnicodeToAscii(string Unicode)
        {
            Unicode = Regex.Replace(Unicode, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[đ|Đ]", "d", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[ý|ỳ|ỷ|ỹ|ỵ|Ý|Ỳ|Ỷ|Ỹ|Ỵ]", "y", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[^A-Za-z0-9-\\s]", "");
            return Unicode;
        }

        public static string ConvertLink(string input, string ch, string ext)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return "";
                }
                input = RemoveUnicode(StandardedCharacters(input));
                input = System.Text.RegularExpressions.Regex.Replace(input, @"[^\w\.@-]", ch) + ext;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return input;
        }
        public static string ConvertLink(string input, string ch = "-")
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return "";
                }
                input = RemoveUnicode(StandardedCharacters(input));
                input = System.Text.RegularExpressions.Regex.Replace(input, @"[^\w\.@-]", ch);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return input;
        }
        public static string UnicodeToUnsignAndDash(string s)
        {
            if (s == null) return string.Empty;

            const string strChar = "abcdefghijklmnopqrstxyzuvxw0123456789 -";
            //string retVal = UnicodeToKoDau(s);
            s = RemoveUnicode(s.ToLower().Trim());
            string sReturn = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (strChar.IndexOf(s[i]) > -1)
                {
                    if (s[i] != ' ')
                        sReturn += s[i];
                    else if (i > 0 && s[i - 1] != ' ' && s[i - 1] != '-')
                        sReturn += "-";
                }
            }
            while (sReturn.IndexOf("--") != -1)
            {
                sReturn = sReturn.Replace("--", "-");
            }
            return sReturn;
        }
        public static string StandardedCharacters(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return string.Empty;
                }
                input = input.Trim();
                while (input.IndexOf("  ") != -1)
                {
                    input = input.Replace("  ", " ");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return input;
        }

        private static string charLower = "aAeEoOuUiIdDyY";
        private static string aLower = "áàạảãâấầậẩẫăắằặẳẵ";
        private static string eLower = "éèẹẻẽêếềệểễeeeeee";
        private static string oLower = "óòọỏõôốồộổỗơớờợởỡ";
        private static string uLower = "úùụủũưứừựửữuuuuuu";
        private static string iLower = "íìịỉĩiiiiiiiiiiii";
        private static string dLower = "đdddddddddddddddd";
        private static string yLower = "ýỳỵỷỹyyyyyyyyyyyy";
        private static string aUpper = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ";
        private static string eUpper = "ÉÈẸẺẼÊẾỀỆỂỄEEEEEE";
        private static string oUpper = "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ";
        private static string uUpper = "ÚÙỤỦŨƯỨỪỰỬỮUUUUUU";
        private static string iUpper = "ÍÌỊỈĨIIIIIIIIIIII";
        private static string dUpper = "ĐDDDDDDDDDDDDDDDD";
        private static string yUpper = "ÝỲỴỶỸYYYYYYYYYYYY";

        public static string RemoveUnicode(string resource)
        {
            string[,] array = new string[14, 18];
            array = initArray();
            string result, temp;
            result = resource;

            for (int i = 1; i < 18; i++)
            {
                array[0, i] = aLower.Substring(i - 1, 1);
                array[1, i] = aUpper.Substring(i - 1, 1);
                array[2, i] = eLower.Substring(i - 1, 1);
                array[3, i] = eUpper.Substring(i - 1, 1);
                array[4, i] = oLower.Substring(i - 1, 1);
                array[5, i] = oUpper.Substring(i - 1, 1);
                array[6, i] = uLower.Substring(i - 1, 1);
                array[7, i] = uUpper.Substring(i - 1, 1);
                array[8, i] = iLower.Substring(i - 1, 1);
                array[9, i] = iUpper.Substring(i - 1, 1);
                array[10, i] = dLower.Substring(i - 1, 1);
                array[11, i] = dUpper.Substring(i - 1, 1);
                array[12, i] = yLower.Substring(i - 1, 1);
                array[13, i] = yUpper.Substring(i - 1, 1);
            }

            for (int j = 0; j < 14; j++)
            {
                for (int k = 0; k < 18; k++)
                {
                    temp = result.Replace(array[j, k], array[j, 0]);
                    result = temp;
                }
            }

            return result;
        }
        public static System.DateTime String2Datetime(string datetime, string CultureInfo = "vi-VN")
        {
            try
            {
                return DateTime.Parse("24/01/2016", new CultureInfo("CultureInfo"));
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return DateTime.Now;
            }
        }

        private static string[,] initArray()
        {
            string[,] array = new string[14, 18];
            for (int i = 0; i < 14; i++)
            {
                array[i, 0] = charLower.Substring(i, 1);
            }
            return array;
        }

        /// <summary>
        /// Hàm tạo mã md5
        /// </summary>
        /// <param name="str">xâu cần mã hóa</param>
        /// <modified>
        /// Author				    created date					comments
        /// duynv					20/08/2016				        Tạo mới
        ///</modified>
        public static string GetMd5x2(string str)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = provider.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte num in bytes)
            {
                builder.Append(num.ToString("x2").ToLower());
            }
            return builder.ToString();
        }

        /// <summary>
        /// Hàm tạo mã md5
        /// </summary>
        /// <param name="str">xâu cần mã hóa</param>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static string GetMd5Sum(string str)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string PublichKey(string str)
        {
            string strCheckSum = string.Empty;


            return "";
        }
        /// <summary>
        /// Hàm mã hóa MD5 theo chuẩn function md5() của ngôn ngữ lập trình php
        /// </summary>
        /// <param name="password">Xâu cần mã hóa</param>
        /// <returns>Xâu đã được mã hóa</returns>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static string GetMd5PHP(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Xóa thẻ HTML
        /// </summary>
        /// <param name="source">xâu có chứa HTML</param>
        /// <modified>
        /// Author				    created date					comments
        /// DuyNV					04/07/2016				        Tạo mới
        ///</modified>
        public static string RemoveHTMLTag(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return Regex.Replace(source, "<.*?>", string.Empty);
            }
            return string.Empty;
            
        }

        /// <summary>
        /// Tạo mật khẩu
        /// </summary>
        /// <param name="leng">Độ dài mật khẩu</param>
        /// <returns></returns>
        public static string RandomString(int leng)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!-@$?()_-";
            char[] chars = new char[leng];
            var rd = new Random();
            for (int i = 0; i < leng; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        /// <summary>
        /// Chuyển giá thành ConvertedPrice khi đúng bằng các cận
        /// </summary>
        /// <param name="price">40.000.000</param>
        /// <param name="convertedPrice">out convertedPrice = 4</param>
        /// <returns>true</returns>
        public static bool ConvertedPriceParse(long price, out int convertedPrice)
        {
            convertedPrice = 0;
            long step = MinMaxValue.StepCovertedPrice;
            if (price % step == 0)
            {
                convertedPrice = (short)(price / step);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Thêm thẻ span vào name bao cụm từ chứa searchValue
        /// </summary>
        /// <param name="inputString">chuỗi text đưa vào</param>
        /// <param name="searchValue">từ khóa</param>
        /// <returns>thêm span bao ngoài từ khóa của chuỗi text</returns>
        public static string FocusingName(string inputString, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue)) return inputString;
            if (string.IsNullOrEmpty(inputString) || !inputString.ToLower().Contains(searchValue.ToLower())) return inputString;
            Regex reg = new Regex(searchValue, RegexOptions.IgnoreCase);
            string outputString = reg.Replace(inputString, delegate (Match m)
            {
                return "<span>" + m.Value + "</span>";
            });
            return outputString;
        }
        public static string FomatPrice(long? price)
        {
            if (!price.HasValue) return string.Empty;
            if (price == MinMaxValue.MaxValuePrice) return string.Empty;
            string s = price.ToString();
            string stmp = s;
            int amount;
            amount = (int)(s.Length / 3);
            if (s.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ".");
            }
            return stmp;
        }
        public static string FomatPrice(double? price)
        {
            if (!price.HasValue) return string.Empty;
            if (price == MinMaxValue.MaxValuePrice) return string.Empty;
            string s = price.ToString();
            string stmp = s;
            int amount;
            amount = (int)(s.Length / 3);
            if (s.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ".");
            }
            return stmp;
        }
        public static string FormatPriceSumary(double? price)
        {
            string pricestring = string.Empty;
            if (price == MinMaxValue.MaxValuePrice) return pricestring;
            if (price >= 1000000000)
            {
                pricestring = string.Format("{0:0.0}", price / 1000000000) + " milyar";
            }
            else if (price >= 1000000)
            {
                pricestring = string.Format("{0:0.0}", price / 1000000) + " juta";
            }
            else
            {
                pricestring = FomatPrice(price);
            }
            return pricestring.Replace(".0 juta", " juta");
        }
        public static string FomatKm(long? km)
        {
            if (!km.HasValue) return string.Empty;
            if (km == MinMaxValue.MaxValueNumOfKm) return string.Empty;
            string s = km.ToString();
            string stmp = s;
            int amount;
            amount = (int)(s.Length / 3);
            if (s.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ".");
            }
            return stmp;
        }
        public static string FomatPrice(string price)
        {
            if (price == "0") return "Negotiable price";
            string stmp = price;
            int amount;
            amount = (int)(price.Length / 3);
            if (price.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ".");
            }
            return stmp;
        }
        public static string FormatPriceSumary(string price)
        {
            if (price == "0" || string.IsNullOrEmpty(price)) return "Negotiable price";
            double dbprice = Double.Parse(price);
            return FormatPriceSumary(dbprice);
        }
        public static string FomatPriceNotUnit(string price)
        {
            string stmp = price;
            int amount;
            amount = (int)(price.Length / 3);
            if (price.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ".");
            }
            return stmp;
        }
        public static string FomatPhone(string phone)
        {
            int amount;
            string stmp = phone;
            amount = (int)(phone.Length / 4);
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(phone.Length - 4 * i, " - ");
            }
            int firstgach = stmp.IndexOf('-');
            if (firstgach == 1)
                stmp = stmp.Substring(firstgach + 1, stmp.Length - firstgach - 1);
            return stmp;
        }
        public static string FomatDate(long createDate)
        {
            DateTime date = new DateTime(createDate);
            return date.ToString("dd/MM/yyyy");
        }

        public static int GetProductIdDB(int id)
        {
            string strId = id.ToString();
            int productid = Convert.ToInt32(strId.Substring(0, strId.Length - 1));
            return productid;
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static bool DelayBeforeContinue(string key, HttpContext context, int delayInSeconds = 5)
        {
            if (context == null) return false;
            System.DateTime now = DateTime.Now;
            if (context.Session.Get(key) != null)
            {
                System.DateTime sessionDateTime;
                if (System.DateTime.TryParse(context.Session.GetString(key), out sessionDateTime))
                {
                    if (sessionDateTime.AddSeconds(delayInSeconds) < now)
                    {
                        context.Session.Remove(key);
                        return false;
                    }
                    else return true;
                }
            }
            else
            {
                context.Session.Set(key, now);
            }
            return false;
        }
        
        public static string GetContentByEvaluationId(string content, int evalId)
        {
            string strRet = string.Empty;
            if (string.IsNullOrEmpty(content))
            {
                return strRet;
            }
            try
            {
                //var doc = new HtmlDocument();
                //doc.LoadHtml(content);
                //foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//*"))
                //{
                //	if (node.Attributes.Contains("eval-id") && node.Attributes["eval-id"].Value.ToInt() == evalId)
                //	{
                //		var nextNode = node.NextSibling;
                //		if (nextNode.Name == "p" || nextNode.Name == "div" || nextNode.Name == "table")
                //		{
                //			strRet += nextNode.OuterHtml;
                //		}
                //	}
                //	if (node.Attributes.Contains("eval-id") && node.Attributes["eval-id"].Value.ToInt() > evalId)
                //	{
                //		break;
                //	}
                //}
                content = content + "<h2 id='0'></h2>";
                var lstMatches = Regex.Matches(content, "<h2 id=\"review-" + evalId + "\".*?>.*?</h2>(?<value>.*?)<h2 id", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (lstMatches.Count > 0)
                {
                    strRet = lstMatches[0].Groups["value"].Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return strRet;
        }
        public static string GetTimeAgoDisplay(System.DateTime date)
        {
            System.DateTime now = System.DateTime.Now;
            if (System.DateTime.Compare(now, date) >= 0)
            {
                TimeSpan s = System.DateTime.Now.Subtract(date);
                TimeSpan w = System.DateTime.Now.StartOfWeek(DayOfWeek.Monday).Subtract(date);

                int numberDays = (now - date).Days;

                int dayDiff = (int)s.TotalDays;
                int weekDiff = System.DateTime.Now.DayOfWeek == DayOfWeek.Monday ? (int)w.TotalDays + 7 : (int)w.TotalDays;

                int secDiff = (int)s.TotalSeconds;

                if (dayDiff == 0)
                {
                    if (secDiff < 60)
                    {
                        return "vừa xong";
                    }
                    if (secDiff < 120)
                    {
                        return "1 phút trước";
                    }
                    if (secDiff < 3600)
                    {
                        return string.Format("{0} phút trước", Math.Floor((double)secDiff / 60));
                    }
                    if (secDiff < 7200)
                    {
                        return "1 giờ trước";
                    }
                    if (secDiff < 86400)
                    {
                        return string.Format("{0} giờ trước", Math.Floor((double)secDiff / 3600));
                    }
                }
                if (numberDays == 1)
                {
                    return "Hôm qua lúc " + date.ToString("HH:mm");
                }
                if (dayDiff > 1 && weekDiff < 7)
                {
                    string dateOfWeek = string.Empty;
                    switch (date.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dateOfWeek = "Thứ hai";
                            break;
                        case DayOfWeek.Tuesday:
                            dateOfWeek = "Thứ ba";
                            break;
                        case DayOfWeek.Wednesday:
                            dateOfWeek = "Thứ tư";
                            break;
                        case DayOfWeek.Thursday:
                            dateOfWeek = "Thứ năm";
                            break;
                        case DayOfWeek.Friday:
                            dateOfWeek = "Thứ sáu";
                            break;
                        case DayOfWeek.Saturday:
                            dateOfWeek = "Thứ bảy";
                            break;
                        case DayOfWeek.Sunday:
                            dateOfWeek = "Chủ nhật";
                            break;
                    }
                    return string.Format("{0} lúc {1}", dateOfWeek, date.ToString("HH:mm"));
                }
                if (dayDiff <= 7)
                {
                    return string.Format("{0} ngày trước", dayDiff);
                }
                //if (dayDiff < 31)
                //{
                //    //return string.Format("{0} tuần trước", Math.Ceiling((double)dayDiff / 7));
                //    return string.Format("{0} ngày trước", dayDiff);
                //}
                //if (dayDiff < 365)
                //{
                //    return string.Format("{0} tháng trước", Math.Ceiling((double)dayDiff / 30));
                //}
                //if (dayDiff >= 365)
                //{
                //    return string.Format("{0} {1}:{2}", date.ToString("dd/MM/yyyy"), date.Hour < 10 ? "0" + date.Hour : date.Hour + "", date.Minute < 10 ? "0" + date.Minute : date.Minute + "");
                //}
                //return date.ToString("dd/MM/yyyy <span>|</span> HH:mm");
                return string.Format("{0}<span>-</span>{1}", date.ToString("HH:mm"), date.ToString("dd/MM/yyyy"));
            }
            else
                return string.Empty;
        }

        public static string GetTimeAgoDisplay(long ticks)
        {
            if (ticks > 0)
            {
                var date = ConvertTicksToDateTime(ticks);
                if (date.HasValue)
                {
                    System.DateTime now = System.DateTime.Now;
                    if (System.DateTime.Compare(now, date.Value) >= 0)
                    {
                        TimeSpan s = System.DateTime.Now.Subtract(date.Value);
                        TimeSpan w = System.DateTime.Now.StartOfWeek(DayOfWeek.Monday).Subtract(date.Value);

                        int numberDays = (now - date).Value.Days;

                        int weekDiff = System.DateTime.Now.DayOfWeek == DayOfWeek.Monday ? (int)w.TotalDays + 7 : (int)w.TotalDays;

                        int dayDiff = (int)s.TotalDays;

                        int secDiff = (int)s.TotalSeconds;

                        if (dayDiff == 0)
                        {
                            if (secDiff < 60)
                            {
                                return "vừa xong";
                            }
                            if (secDiff < 120)
                            {
                                return "1 phút trước";
                            }
                            if (secDiff < 3600)
                            {
                                return string.Format("{0} phút trước", Math.Floor((double)secDiff / 60));
                            }
                            if (secDiff < 7200)
                            {
                                return "1 giờ trước";
                            }
                            if (secDiff < 86400)
                            {
                                return string.Format("{0} giờ trước", Math.Floor((double)secDiff / 3600));
                            }
                        }
                        if (numberDays == 1)
                        {
                            return "Hôm qua lúc " + date.Value.ToString("HH:mm");
                        }
                        if (dayDiff > 1 && weekDiff < 7)
                        {
                            string dateOfWeek = string.Empty;
                            switch (date.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    dateOfWeek = "Thứ Hai";
                                    break;
                                case DayOfWeek.Tuesday:
                                    dateOfWeek = "Thứ Ba";
                                    break;
                                case DayOfWeek.Wednesday:
                                    dateOfWeek = "Thứ Tư";
                                    break;
                                case DayOfWeek.Thursday:
                                    dateOfWeek = "Thứ Năm";
                                    break;
                                case DayOfWeek.Friday:
                                    dateOfWeek = "Thứ Sáu";
                                    break;
                                case DayOfWeek.Saturday:
                                    dateOfWeek = "Thứ Bảy";
                                    break;
                                case DayOfWeek.Sunday:
                                    dateOfWeek = "Chủ Nhật";
                                    break;
                            }
                            return string.Format("{0} lúc {1}", dateOfWeek, date.Value.ToString("HH:mm"));
                        }
                        if (dayDiff <= 7)
                        {
                            return string.Format("{0} ngày trước", dayDiff);
                        }
                        //if (dayDiff < 31)
                        //{
                        //    //return string.Format("{0} tuần trước", Math.Ceiling((double)dayDiff / 7));
                        //    return string.Format("{0} ngày trước", dayDiff);
                        //}
                        //if (dayDiff < 365)
                        //{
                        //    return string.Format("{0} tháng trước", Math.Ceiling((double)dayDiff / 30));
                        //}
                        //if (dayDiff >= 365)
                        //{
                        //    return string.Format("{0} {1}:{2}", date.Value.ToString("dd/MM/yyyy"), date.Value.Hour < 10 ? "0" + date.Value.Hour : date.Value.Hour + "", date.Value.Minute < 10 ? "0" + date.Value.Minute : date.Value.Minute + "");
                        //}
                        return string.Format("{0} {1}:{2}", date.Value.ToString("dd/MM/yyyy"), date.Value.Hour < 10 ? "0" + date.Value.Hour : date.Value.Hour + "", date.Value.Minute < 10 ? "0" + date.Value.Minute : date.Value.Minute + "");
                    }
                    else
                        return string.Empty;
                }
            }
            return string.Empty;
        }

        public static System.DateTime StartOfWeek(this System.DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = startOfWeek - dt.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(diff).Date;
        }

        public static string GetTimeDisplayByDay(long ticks)
        {
            if (ticks > 0)
            {
                var date = ConvertTicksToDateTime(ticks);
                if (date.HasValue)
                {
                    System.DateTime now = System.DateTime.Now;
                    if (System.DateTime.Compare(now, date.Value) >= 0)
                    {
                        TimeSpan s = System.DateTime.Now.Subtract(date.Value);

                        int dayDiff = (int)s.TotalDays;

                        int secDiff = (int)s.TotalSeconds;

                        if (dayDiff == 0)
                        {
                            if (secDiff < 60)
                            {
                                return "vừa xong";
                            }
                            if (secDiff < 120)
                            {
                                return "1 phút trước";
                            }
                            if (secDiff < 3600)
                            {
                                return string.Format("{0} phút trước", Math.Floor((double)secDiff / 60));
                            }
                            if (secDiff < 7200)
                            {
                                return "1 giờ trước";
                            }
                            if (secDiff < 86400)
                            {
                                return string.Format("{0} giờ trước", Math.Floor((double)secDiff / 3600));
                            }
                        }
                        else if (dayDiff == 1)
                        {
                            return date.Value.ToString("HH:mm") + " - Hôm qua";
                        }
                        else
                        {
                            return string.Format("{0}:{1} - {2}", date.Value.Hour < 10 ? "0" + date.Value.Hour : date.Value.Hour + "", date.Value.Minute < 10 ? "0" + date.Value.Minute : date.Value.Minute + "", date.Value.ToString("dd/MM/yyyy"));
                        }
                        return string.Empty;
                    }
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static System.DateTime? ConvertTicksToDateTime(long lticks)
        {
            if (lticks == 0) return null;
            return new System.DateTime(lticks);
        }

        public static string ConvertTicksToStringFormat(long ticks)
        {
            if (ticks == 0) return string.Empty;
            var convertDate = ConvertTicksToDateTime(ticks);
            var strDateFormat = "{0} {1}";
            if (!convertDate.HasValue) return string.Empty;

            return string.Format(strDateFormat, convertDate.Value.ToString("dd/MM/yyyy"), convertDate.Value.ToString("HH:mm"));

        }

        public static string ConvertTicksToStringFormatWithDash(long ticks)
        {
            if (ticks == 0) return string.Empty;
            var convertDate = ConvertTicksToDateTime(ticks);
            var strDateFormat = "{0} - {1}";
            if (!convertDate.HasValue) return string.Empty;

            return string.Format(strDateFormat, convertDate.Value.ToString("dd/MM/yyyy"), convertDate.Value.ToString("HH:mm"));

        }

        public static string ConvertTicksToStringFormat(long ticks, string formatString)
        {
            if (ticks == 0) return string.Empty;
            var convertDate = ConvertTicksToDateTime(ticks);
            var strDateFormat = "{0}";
            if (!convertDate.HasValue) return string.Empty;
            return string.Format(strDateFormat, convertDate.Value.ToString(formatString));
        }

        public static string ConvertTicksToStringTimeFormat(long ticks)
        {
            if (ticks == 0) return string.Empty;
            var prefix = "";
            var convertDate = ConvertTicksToDateTime(ticks);
            if (!convertDate.HasValue) return string.Empty;

            int hour = convertDate.Value.Hour;
            if (hour >= 0 && hour <= 12) prefix = "sáng";
            if (hour >= 13 && hour <= 17) prefix = "chiều";
            if (hour >= 18 && hour <= 23) prefix = "tối";
            var strDateFormat = "lúc {0} " + prefix;
            return string.Format(strDateFormat, convertDate.Value.ToString("HH:mm"));
        }

        public static System.DateTime ConvertStringToDateTime(string value, string format)
        {
            return System.DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }

        public static System.DateTime ConvertToDateTime(object value)
        {
            System.DateTime returnValue;

            if (null == value || !System.DateTime.TryParse(value.ToString(), out returnValue))
            {
                returnValue = System.DateTime.MinValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Return double, > 0 then dateTo > dateFrom
        /// </summary>
        /// <pre>instant: d = Date, h = Hour, m = Minute, s = Second</pre>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="instant"></param>
        /// <returns></returns>
        public static double DateDiff(System.DateTime dateFrom, System.DateTime dateTo, string instant)
        {
            TimeSpan span = (TimeSpan)(dateTo - dateFrom);
            double num = 0.0;
            string str = instant.ToLower();
            if (str == null)
            {
                return num;
            }
            if (str != "d")
            {
                if (str != "h")
                {
                    if (str == "m")
                    {
                        return span.TotalMinutes;
                    }
                    if (str != "s")
                    {
                        return num;
                    }
                    return span.TotalSeconds;
                }
            }
            else
            {
                return span.TotalDays;
            }
            return span.TotalHours;
        }

        public static long DateTimeToUnixTime(System.DateTime dateTime)
        {
            System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan)(dateTime - epoch.ToLocalTime());
            return (long)(span.TotalSeconds * 1000.0);
        }

        public static long DateTimeToUnixTimeDaily(System.DateTime dateTime)
        {
            dateTime = System.DateTime.Parse(dateTime.ToString("MM/dd/yyyy 00:00:00"));
            System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan)(dateTime.Date - epoch.ToLocalTime());
            return (long)(span.TotalSeconds * 1000.0);
        }

        public static System.DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp / 1000.0).ToLocalTime();
            return dtDateTime;
        }

        public static long DateTimeToSpanHourly(System.DateTime dateTime)
        {
            dateTime = System.DateTime.Parse(dateTime.ToString("MM/dd/yyyy HH:00:00"));
            System.DateTime time = new System.DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan)(dateTime - time.ToLocalTime());
            return (long)(span.TotalSeconds * 1000.0);
        }

        public static bool IsLong(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            long longConverted = 0;
            if (long.TryParse(input, out longConverted)) return true;
            return false;
        }

        public static bool IsInteger(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            int integerConverted = 0;
            if (int.TryParse(input, out integerConverted)) return true;
            return false;
        }

        public static bool IsSizeFormat(string input)
        {

            if (input.IndexOf("x") != -1)
            {
                string[] sizes = input.Split('x');
                if (sizes.Length != 3) return false;
                else
                {
                    foreach (var size in sizes)
                    {
                        if (!IsInteger(size)) return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public static string MySubString(string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            StringBuilder __returnStr = new StringBuilder();
            string[] __separator = new string[] { " " };
            string[] __arrStr = input.Split(__separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < __arrStr.Length; i++)
            {
                if (i < length && i < __arrStr.Length)
                {
                    __returnStr.Append(__arrStr[i] + " ");

                }
                else
                {
                    break;
                }
            }
            return __returnStr.ToString();
        }

        public static string MySubStringNotHtml(string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            String regex = "<a(.*?)</a>";
            Match math = Regex.Match(input, regex);
            String result = string.Empty;
            if (math.Success)
            {
                result = math.Groups[0].Value;
                input = Regex.Replace(input, "<a(.*?)</a>", "_temphtml_");
            }
            StringBuilder __returnStr = new StringBuilder();
            string[] __separator = new string[] { " " };
            string[] __arrStr = input.Split(__separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < __arrStr.Length; i++)
            {
                if (i < length && i < __arrStr.Length)
                {
                    __returnStr.Append(__arrStr[i] + " ");

                }
                else
                {
                    break;
                }
            }
            return __returnStr.Replace("_temphtml_", result).ToString();
        }

        public static string MySubStringWithDot(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length <= length)
            {
                return MySubStringNotHtml(input, length);
            }

            return string.Concat(MySubStringNotHtml(input, length).Trim(), "...");
        }

        public static string MySubCharater(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length <= length)
            {
                return input;
            }

            return input.Substring(0, length) + "...";
        }

        public static string PreProcessSearchString(string searchString)
        {
            string output = searchString.Replace("'", " ").Replace("\"\"", " ").Replace(">", " ").Replace("<", " ").Replace(",", " ").Replace("(", " ").Replace(")", " ").Replace("\"", " ");
            output = Regex.Replace(output, "[ ]+", "+");
            return output.Trim();
        }

        public static string GetMACAddress()
        {
            string str = string.Empty;
            try
            {
                NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface interface2 in allNetworkInterfaces)
                {
                    if (str == string.Empty)
                    {
                        interface2.GetIPProperties();
                        str = interface2.GetPhysicalAddress().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // no
            }
            return str;
        }

        public static string GetCssClassByTopicId(int topicId)
        {
            switch (topicId)
            {
                case 1:
                    return "boxtitle-furum";
                case 6:
                    return "boxtitle-tuvan";
                case 13:
                    return "boxtitle-baoduong";
                case 25:
                    return "boxtitle-traodoi";
                case 34:
                    return "boxtitle-giaoluu";
                default:
                    return "";
            }
        }

        public static string GetCurrentURL(string url, int? pageIndex)
        {
            if (url.IndexOf("/p" + pageIndex) != -1)
            {
                if (url.Substring(url.Length - (pageIndex.ToString().Length + 2)) == "/p" + pageIndex) return url.Substring(0, url.Length - (pageIndex.ToString().Length + 2));
            }
            return url;
        }

        public static int ConvertEnumToInt(Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static string GenPaggingAjax(string functionName, int pageIndex, int pageSize, int totalCount)
        {
            int pageNum = (int)Math.Ceiling((double)totalCount / pageSize);
            if (pageNum * pageSize < totalCount)
            {
                pageNum++;
            }
            string htmlPage = string.Empty;
            const string buildlink = "<li><a href=\"javascript:{0}('{1}')\" class=\"{2}\" title=\"{4}\">{3}</a></li>";
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            string currentPage = pageIndex.ToString(); // trang tiện tại
            int iCurrentPage = 0;
            if (pageIndex <= 0) iCurrentPage = 1;
            else iCurrentPage = pageIndex;
            string active;

            if (pageNum >= 2)
            {
                if (iCurrentPage == 1)
                {
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-double-left'></i>", "Trang đầu tiên");
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-left'></i>", "Trang trước");
                }
                else
                {
                    if ((iCurrentPage - 1) == 1)
                    {

                        htmlPage += String.Format(buildlink, functionName, string.Empty, string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang đầu tiên");
                        htmlPage += String.Format(buildlink, functionName, string.Empty, string.Empty, "<i class='fa fa-angle-left'></i>", "Trang trước");
                    }

                    else
                    {
                        htmlPage += String.Format(buildlink, functionName, 1, string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang đầu tiên");
                        htmlPage += String.Format(buildlink, functionName, (iCurrentPage - 1), string.Empty, "<i class='fa fa-angle-left'></i>", "Trang trước");
                    }
                }
            }

            if (pageNum <= 4)
            {
                if (pageNum != 1)
                {

                    for (int i = 1; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        if (i == 1) htmlPage += String.Format(buildlink, functionName, string.Empty, active, i, "Trang " + i);
                        else htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);
                    }
                }
            }
            else
            {
                if (iCurrentPage < (pageNum - 3))
                {
                    if (iCurrentPage <= 3)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            if (i == 1) htmlPage += String.Format(buildlink, functionName, string.Empty, active, i, "Trang " + i);
                            else htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);
                        }
                    }
                    else
                    {
                        for (int i = iCurrentPage - 2; i <= iCurrentPage + 2; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);
                        }
                    }
                }
                else
                {
                    for (int i = pageNum - 3; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);

                    }
                }

            }
            if (pageNum >= 2)
            {
                if (iCurrentPage == pageNum)
                {
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-right'></i>", "Trang sau");
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-double-right'></i>", "Trang cuối");
                }
                else
                {
                    htmlPage += String.Format(buildlink, functionName, (iCurrentPage + 1), string.Empty, "<i class='fa fa-angle-right'></i>", "Trang sau");
                    htmlPage += String.Format(buildlink, functionName, pageNum, string.Empty, "<i class='fa fa-angle-double-right'></i>", "Trang cuối");
                }
            }
            htmlPage = string.Format("<div class=\"paging\"><ul>{0}</ul></div>", htmlPage);
            return htmlPage;
        }

        public static string GenPagging(int pageIndex, int pageSize, string linkSite, int count, string paramNamePaging = "p")
        {
            int pageNum = (int)Math.Ceiling((double)count / pageSize);
            if (pageNum * pageSize < count)
            {
                pageNum++;
            }

            string prefix = linkSite.Contains("?") ? linkSite.Substring(linkSite.LastIndexOf("?", StringComparison.Ordinal), linkSite.Length - linkSite.LastIndexOf("?", StringComparison.Ordinal)).TrimEnd('/') : "";

            string strLinkSite = linkSite.Contains("?") ? Regex.Replace(linkSite.Substring(0, linkSite.LastIndexOf("?", StringComparison.Ordinal)), @"p\d+", "").TrimEnd('/') + "/" : linkSite.TrimEnd('/') + "/";

            linkSite = strLinkSite;

            string htmlPage = string.Empty;
            string buildlink = "<li class='{2}'><a href='{0}{1}' title='{4}'>{3}</a></li>";
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            string currentPage = pageIndex.ToString(); // trang tiện tại
            int iCurrentPage = 0;
            if (pageIndex <= 0) iCurrentPage = 1;
            else iCurrentPage = pageIndex;
            string active;
            if (pageNum >= 2)
            {
                if (iCurrentPage == 1)
                {
                    htmlPage += String.Format(buildlink, "javascript:;", string.Empty, "hint", "<i class='fa fa-angle-double-left'></i>", "Trang trước");
                }
                else
                {
                    if ((iCurrentPage - 1) == 1)
                        htmlPage += string.Format(buildlink, linkSite.TrimEnd('/'), string.Empty + prefix, string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang trước");
                    else
                        htmlPage += string.Format(buildlink, linkSite, prefix + paramNamePaging + (iCurrentPage - 1), string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang trước");
                }
            }
            if (pageNum <= 4)
            {
                if (pageNum != 1)
                {

                    for (int i = 1; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        if (i == 1) htmlPage += String.Format(buildlink, linkSite.TrimEnd('/'), string.Empty + prefix, active, i, "Trang " + i);
                        else htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);
                    }
                }
            }
            else
            {
                if (iCurrentPage < (pageNum - 3))
                {
                    if (iCurrentPage <= 3)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            if (i == 1) htmlPage += String.Format(buildlink, linkSite.TrimEnd('/'), string.Empty + prefix, active, i, "Trang " + i);
                            else htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);
                        }
                    }
                    else
                    {
                        for (int i = iCurrentPage - 2; i <= iCurrentPage + 2; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);
                        }
                    }
                }
                else
                {
                    for (int i = pageNum - 3; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);

                    }
                }

            }
            if (pageNum >= 2)
            {
                if (iCurrentPage == pageNum)
                {
                    htmlPage += String.Format(buildlink, "javascript:;", string.Empty, "hint", "<i class='fa fa-angle-double-right'></i>", "Trang sau");
                }
                else
                {
                    htmlPage += String.Format(buildlink, linkSite, paramNamePaging + (iCurrentPage + 1) + prefix, string.Empty, "<i class='fa fa-angle-double-right'></i>", "Trang sau");
                }
            }
            htmlPage = string.Format("<div class='paging'><ul>{0}</ul></div>", htmlPage);
            return htmlPage;
        }

        /*
        public static string GenarateCanonicalUrl(string standardUrl)
        {
            string strHostName = AppSettings.Instance.GetString("HostName");
            string strHostNameMobile = AppSettings.Instance.GetString("MobileHostName");
            string strBaseUrl = AppSettings.Instance.GetString("BaseUrl");
            string strBaseUrlMobile = AppSettings.Instance.GetString("MobileBaseUrl");

            bool isMobile = DetectDevice.Instance.BrowserIsMobile();
            string hostRequest = HttpContext.Current.Request.Url.Host.ToLower();

            hostRequest = hostRequest.Replace(string.Format(":{0}", HttpContext.Current.Request.Url.Port), string.Empty);

            string hostConfig = strHostName.ToLower();
            string mobileHostConfig = strHostNameMobile.ToLower();

            string baseUrl = strBaseUrl.ToLower();
            string mobileBaseUrl = strBaseUrlMobile.ToLower();

            if (hostConfig != mobileHostConfig)
            {
                if (hostRequest.Equals(hostConfig))
                {
                    isMobile = false;
                }
                else if (hostRequest.Equals(mobileHostConfig))
                {
                    isMobile = true;
                }
            }
            if (isMobile)
            {
                standardUrl = baseUrl + standardUrl.Replace(mobileHostConfig, string.Empty);
            }
            else
            {
                standardUrl = mobileBaseUrl + standardUrl.Replace(hostConfig, string.Empty);
            }
            return standardUrl;
        }
        */

        /// <summary>
        /// Decodes the query parameters.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">uri</exception>
        public static Dictionary<string, string> DecodeQueryParameters(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Query.Length == 0)
                return new Dictionary<string, string>();

            return uri.Query.TrimStart('?')
                            .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(parts => parts[0],
                                        parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                            .ToDictionary(grouping => grouping.Key,
                                            grouping => string.Join(",", grouping));
        }

        /// <summary>
        /// Dictionaries to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        public static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // change the type
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }

        /// <summary>
        /// Replaces the case insensitive.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="search">The search.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>

        public static long CreateIdAsInt64()
        {
            System.DateTime dateTime = System.DateTime.Now;

            System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = dateTime - epoch.ToLocalTime();
            long output = (long)(span.TotalMilliseconds);

            return output;
        }

        public static int CreateIdAsInt32()
        {
            Random rand = new Random();

            int output = rand.Next(1, int.MaxValue);

            return output;
        }

        public static bool IsNewsId(string str, out long newsId)
        {
            newsId = 0;
            if (str.Length == 17)
            {
                newsId = str.ToLong();
                if (newsId > 0) return true;
            }
            return false;
        }

        public static System.DateTime EndOfDay(this System.DateTime date)
        {
            return new System.DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 000);
        }

        public static System.DateTime StartOfDay(this System.DateTime date)
        {
            return new System.DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
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


        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static string GetMetaDescription(string description)
        {
            var des = RemoveHTMLTag(description);
            if (des.Length > 160)
                des.Substring(0, 160);
            return des;
        }

        public static IEnumerable<SelectListItem> EnumToSelectList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = Convert.ToInt32(v).ToString()
            }).ToList();
        }

        public static IEnumerable<SelectListItem> EnumToSelectListDes<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItem
            {
                Text = StringUtils.GetEnumDescription((Enum)Enum.Parse(typeof(T), v.ToString(), true)),
                Value = Convert.ToInt32(v).ToString()
            }).ToList();
        }

        public static string MakeGetRequest(string url, string username = "", string password = "")
        {
            string data = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-ms-application, image/jpeg, application/xaml+xml";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                request.Credentials = new NetworkCredential(username, password);
            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return data;
        }
    }
}
