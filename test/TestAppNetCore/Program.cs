using Camefor.Tools.NetCore.Util;
using System;

namespace TestAppNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var dt = DateTime.Now;
            Console.WriteLine($"系统当前时间: {dt.ToString("yyyy-MM-dd HH:mm:ss")}");

            var unixTimestamp = dt.DateConvertToUnixTimestamp();
            Console.WriteLine($"当前时间转换为时间戳后为： {unixTimestamp}");

            var convertedDt = unixTimestamp.UnixTimestampConvertToDate().ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"时间戳： {unixTimestamp} 转换为时间类型为：{convertedDt}");

        }
    }
}
