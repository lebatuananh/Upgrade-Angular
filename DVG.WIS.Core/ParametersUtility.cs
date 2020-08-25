using System;
using System.Text.RegularExpressions;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;

namespace DVG.WIS.Core
{
    public class ParametersUtility
    {
        private static object lockedObject = new object();
        private static ParametersUtility _instance;
        public static ParametersUtility Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (lockedObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new ParametersUtility();
                        }
                    }
                }
                return _instance;
            }
        }

        public double ConvertParamToMoney(string input)
        {
            double output = 0;

            if (string.IsNullOrEmpty(input)) return output;

            try
            {
                string strRegex = @"(?<number>[0-9.,]+)(?<unit>tr|ty|m|b)?";
                Regex myRegex = new Regex(strRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = myRegex.Match(input);

                if (match.Success)
                {
                    double number = Convert.ToDouble(match.Groups["number"].Value);
                    string unit = Convert.ToString(match.Groups["unit"].Value);

                    switch (unit)
                    {
                        case "tr":// triệu
                        case "trieu":// triệu
                        case "m":// million
                            number = number * 1000000; // 1 triệu
                            break;
                        case "ty":// tỷ
                        case "b":// billion
                            number = number * 1000000000; // 1 tỷ
                            break;
                        default:
                            //number = number;
                            break;
                    }

                    output = number;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return output;
        }

        public int ConvertParamToMonth(string input)
        {
            int output = 0;

            if (string.IsNullOrEmpty(input)) return output;

            try
            {
                string strRegex = @"(?<number>[0-9.,]+)(?<unit>th|thang|nam|year)?";
                Regex myRegex = new Regex(strRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = myRegex.Match(input);

                if (match.Success)
                {
                    int number = Convert.ToInt32(match.Groups["number"].Value);
                    string unit = Convert.ToString(match.Groups["unit"].Value);

                    switch (unit)
                    {
                        case "nam":
                        case "year":
                            number = number * 12;
                            break;
                        case "thang":
                        case "th":
                        default:
                            //number = number;
                            break;
                    }

                    output = number;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return output;
        }

        public int ConvertParamToYear(string input)
        {
            int output = 0;

            if (string.IsNullOrEmpty(input)) return output;

            try
            {
                string strRegex = @"(?<number>[0-9.,]+)(?<unit>th|thang|nam|year)?";
                Regex myRegex = new Regex(strRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = myRegex.Match(input);

                if (match.Success)
                {
                    int number = Convert.ToInt32(match.Groups["number"].Value);
                    string unit = Convert.ToString(match.Groups["unit"].Value);

                    switch (unit)
                    {
                        case "thang":
                        case "th":
                            number = Math.Round(Convert.ToDecimal(number / 12), 0).ToInt();
                            break;
                        case "nam":
                        case "year":
                        default:
                            //number = number;
                            break;
                    }

                    output = number;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return output;
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

        public static string ConvertDenominationsCurrency(double value, out string unit) //chuyển đổi mệnh giá tiền
        {
            unit = "";
            string str = "";
            if (value >= 1000000000)
            {
                str = Math.Round(value / 1000000000, 2).ToString();
                unit = "tỷ";
            }
            else if (value >= 1000000)
            {
                str = Math.Round(value / 1000000, 2).ToString();
                unit = "triệu";
            }
            else
            {
                str = Math.Round(value / 1000).ToString();
                unit = "nghìn";
            }
            return str;
        }
    }
}
