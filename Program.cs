using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Job
{
  class Program
  {
    static void Main(string[] args)
    {
      var host = new HostBuilder()
      .ConfigureServices((hostContext, services) =>
      {
        //注册后台THostedService类型服务
        services.AddHostedService<LogJob>();
      })
      .Build();
      host.Run();
    }
  }
}
