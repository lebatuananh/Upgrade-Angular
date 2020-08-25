using DVG.WIS.Business.AhaMove;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.AhaMove;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace DVG.WIS.Business.xTest
{
    public class AhaMoveTest : BoTest
    {
        [Fact]
        public void AhaMoveCreateOrderTest()
        {
            PathInfo kitchenPath = new PathInfo { Address = "53 Lê Anh Xuân, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh" };
            PathInfo customerPath = new PathInfo { Address = "5 Nguyễn Gia Thiều, Phường 6, Quận 3, Thành phố Hồ Chí Minh", Name = "Guest", Mobile = "8954342123" };
            List<OrderDetail> orderDetails = new List<OrderDetail> {
                new OrderDetail{product_id = 1, product_name = "Sản phẩm 1", price = 50000, quantity = 2},
                new OrderDetail{product_id = 2, product_name = "Sản phẩm 2", price = 80000, quantity = 3}
            };
            var response = AhaMoveApi.CreateOrder(kitchenPath, customerPath, orderDetails);
            Console.WriteLine(response);
        }

        [Fact]
        public void OrderAgentRunTest()
        {
            var orderAgent = new OrderAgent();
            orderAgent.Run();
            Assert.True(true);
        }

        static char[] baseChars = new char[36] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        const int Base36Size = 36;
        public static string IntToBase36(long value)
        {
            //char[] baseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            //string result = string.Empty;
            StringBuilder sbNumber = new StringBuilder();
            //int targetBase = baseChars.Length;
            do
            {
                sbNumber.Insert(0, baseChars[value % Base36Size]);
                //result = baseChars[value % Base36Size] + result;
                value = value / Base36Size;
            }
            while (value > 0);

            //return result;
            return sbNumber.ToString();
        }

        [Fact]
        public void GenerateTest()
        {
            var orderFormat = "'{0}','{1}','{2}','{3}',{4}";
            var pathInt = @"D:\Databases\orders-int.csv";
            var pathChar = @"D:\Databases\orders-char.csv";

            var swInt = new StreamWriter(pathInt);
            var swChar = new StreamWriter(pathChar);
            for(var i = 0; i < 2000000; i++)
            {
                var unixTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var orderCharId = IntToBase36(unixTimestamp);
                swInt.WriteLine(string.Format(orderFormat, unixTimestamp, "Customer " + i, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff"), "Note " + i, 1));
                swChar.WriteLine(string.Format(orderFormat, orderCharId, "Customer " + i, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff"), "Note " + i, 1));
                Thread.Sleep(5);
            }
            swInt.Flush();
            swInt.Close();
            swChar.Flush();
            swChar.Close();
        }

        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalMilliseconds;
        }

        [Fact]
        public void GenerateIdTest()
        {
            //var random = new Random(5000);
            var pathInt = @"D:\Databases\random-uid.csv";
            var swInt = new StreamWriter(pathInt);
            var datetime = DateTime.Now;
            for (var i = 0; i < 5000000; i++)
            {
                datetime = datetime.AddSeconds(1);
                var timestamp = ConvertToUnixTime(datetime);
                //var id = random.Next(100000000, 1799999999);
                var uid = StringUtils.IntToBase36(timestamp);
                swInt.WriteLine(uid);
            }
            swInt.Flush();
            swInt.Close();
        }
    }
}
