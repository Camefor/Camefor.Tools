using System;

/// <summary>
/// https://docs.microsoft.com/zh-cn/dotnet/standard/net-standard
/// .NET Framework 4.6.1
/// .NET Core  2.0	3.0
/// .NET 5
/// </summary>
namespace Camefor.Tools.NetCore
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
