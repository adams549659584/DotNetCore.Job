using System;
using System.IO;
using System.Net.Http;

namespace DotNetCore.Job
{
  public class IpPushJob : BaseJob
  {
    private static TimeSpan JobTimerInterval = TimeSpan.FromMinutes(10);
    public IpPushJob() : base(JobTimerInterval)
    {
      Console.WriteLine("IP推送作业启动");
    }

    protected override void DoWork(object state)
    {
      Console.WriteLine("IP推送作业执行：");
      IpPush();
    }

    static void IpPush()
    {
      try
      {
        string resultStr = HttpGet("http://ip-api.com/line/?fields=query");
        string oldIp = GetIpCache();
        if (oldIp == resultStr)
        {
          WriteLog(string.Format("IP 【{0}】未变更，无需通知", oldIp));
        }
        else
        {
          string ip = resultStr.Trim();
          SetIpCache(ip);
          WriteLog(string.Format("IP 【{0}】已变更为 【{1}】，需通知", oldIp, resultStr));
          string notifyResult = HttpGet(string.Format("https://sc.ftqq.com/SCU33276T4801adab529b3595e3dc25d37cbe38a35bb5f40021bbd.send?text=您的外网ip变更为{0}了", ip));
          WriteLog(notifyResult);
        }
      }
      catch (Exception ex)
      {
        WriteLog(ex.ToString(), Path.Combine("", "ex.log"));
      }
    }

    static string HttpGet(string url)
    {
      using (HttpClient httpClient = new HttpClient())
      {
        HttpResponseMessage response = httpClient.GetAsync(url).Result;
        string resultStr = response.Content.ReadAsStringAsync().Result.Trim();
        return resultStr;
      }
    }

    static string GetIpCache()
    {
      string ipPath = @"C:\IpLog\ip.txt";
      string ip = string.Empty;
      using (StreamReader sr = new StreamReader(ipPath))
      {
        ip = sr.ReadToEnd();
      }
      return ip.Trim();
    }
    static void SetIpCache(string ip)
    {
      string ipPath = @"C:\IpLog\ip.txt";
      using (StreamWriter sw = new StreamWriter(ipPath))
      {
        sw.WriteLine(ip);
      }
    }

    static void WriteLog(string log, string logPath = "")
    {
      if (string.IsNullOrWhiteSpace(logPath))
      {
        logPath = string.Format("C:\\IpLog\\Log\\{0}.log", DateTime.Now.ToString("yyyyMMddHH"));
      }
      using (StreamWriter sw = new StreamWriter(logPath, true))
      {
        sw.WriteLine(log);
      }
    }
  }
}