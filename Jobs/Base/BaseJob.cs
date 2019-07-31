using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DotNetCore.Job
{
  public abstract class BaseJob : IHostedService, IDisposable
  {
    private Timer _timer;
    private TimeSpan _timerInterval;

    /// <summary>
    /// 作业基类
    /// </summary>
    /// <param name="timerInterval">作业运行的时间间隔</param>
    public BaseJob(TimeSpan timerInterval)
    {
      _timerInterval = timerInterval;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _timer = new Timer(DoWork, null, TimeSpan.Zero, _timerInterval);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _timer?.Change(Timeout.Infinite, 0);
      return Task.CompletedTask;
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="state"></param>
    protected abstract void DoWork(object state);
  }
}