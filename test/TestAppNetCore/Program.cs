using Camefor.Tools.NetCore.Util;
using System;

namespace TestAppNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var dt = DateTime.Now;
            var dt = Convert.ToDateTime("2022-03-02 15:40:28");
            Console.WriteLine($"系统当前时间: {dt.ToString("yyyy-MM-dd HH:mm:ss")}");

            //var unixTimestamp = dt.DateConvertToUnixTimestamp();
            var unixTimestamp = 1646235949D;
            Console.WriteLine($"当前时间转换为时间戳后为： {unixTimestamp}");

            var convertedDt = unixTimestamp.UnixTimestampConvertToDate().ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"时间戳： {unixTimestamp} 转换为时间类型为：{convertedDt}");

        }
    }
}
