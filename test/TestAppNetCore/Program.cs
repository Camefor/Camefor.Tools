using Camefor.Tools.NetCore.Util.Web;
using Camefor.Tools.NetCore.Util.Log;
using System;
using System.Threading.Tasks;

namespace TestAppNetCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var dt = DateTime.Now;
            //var dt = Convert.ToDateTime("2022-03-02 15:40:28");
            //Console.WriteLine($"系统当前时间: {dt.ToString("yyyy-MM-dd HH:mm:ss")}");

            ////var unixTimestamp = dt.DateConvertToUnixTimestamp();
            //var unixTimestamp = 1646235949D;
            //Console.WriteLine($"当前时间转换为时间戳后为： {unixTimestamp}");

            //var convertedDt = unixTimestamp.UnixTimestampConvertToDate().ToString("yyyy-MM-dd HH:mm:ss");
            //Console.WriteLine($"时间戳： {unixTimestamp} 转换为时间类型为：{convertedDt}");


            //EventLog eventLog = new EventLog();
            //eventLog.AddLog("测试", "hello");


            //EventLog eventLog1 = new EventLog(true, "测试222");
            //eventLog1.AddLog("测试sss", "hello");


            var url = @"http://49.232.106.166:5244/d/myfiles/2009-11-13李志动物凶猛全国巡演郑州站—《关于郑州的记忆》.mp4";
            var stream = await HttpMethods.GetForDetail(url);

            Console.WriteLine("请求结束");

        }
    }
}
