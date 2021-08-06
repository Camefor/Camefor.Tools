using System;



/// <summary>
/// 只兼容 .NET Framework 4.5 ,只有不能使用.NET 4.6.1时使用， 不对 .net core 实现
/// </summary>
namespace Camefor.Tools.Net45
{
    public class TestTool
    {
        public void SayHello()
        {
            var version = Environment.Version;
            Console.WriteLine($"Hello World {version}");
        }
    }
}
