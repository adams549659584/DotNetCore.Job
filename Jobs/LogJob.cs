using System;
using System.IO;
using System.Text;

namespace DotNetCore.Job
{
  public class LogJob : BaseJob
  {
    private static TimeSpan JobTimerInterval = TimeSpan.FromSeconds(5);
    public LogJob() : base(JobTimerInterval)
    {
      Console.WriteLine("日志作业启动");
    }

    protected override void DoWork(object state)
    {
      Console.WriteLine("日志作业执行：");
      Log();
    }

    void Log()
    {
      var fullPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, "log");
      if (!Directory.Exists(fullPath))
      {
        Directory.CreateDirectory(fullPath);
      }
      var fullName = Path.Combine(fullPath, string.Format("{0}.log", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
      using (StreamWriter sw = new StreamWriter(fullName, true, Encoding.UTF8))
      {
        var log = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Console.WriteLine(log);
        sw.WriteLine(log);
      }
    }
  }
}