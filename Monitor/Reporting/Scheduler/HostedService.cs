using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Monitor.Reporting.Scheduler
{
    public class HostedService : IHostedService
    {
        private IScheduledTask _taskToExecute;
        private Task _executingTask;
        private CancellationTokenSource _cts;
        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        public HostedService(IScheduledTask scheduledTask)
        {
            _taskToExecute = scheduledTask;
            UnobservedTaskException = (sender, args) =>
            {
                System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "ErrorReport.txt", System.DateTime.Now + "\t" + args.Exception.Message);
                args.SetObserved();
            };
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = ExecuteAsync(_cts.Token);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_taskToExecute == null)
            {
                return;
            }
            // Signal cancellation to the executing method
            _cts.Cancel();

            // Wait until the task completes or the stop token triggers
            await  _executingTask;
            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }
        // Derived classes should override this and execute a long running method until 
        // cancellation is requested
        protected async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _taskToExecute.ExecuteAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    var args = new UnobservedTaskExceptionEventArgs(
                        ex as AggregateException ?? new AggregateException(ex));

                    UnobservedTaskException?.Invoke(this, args);

                    if (!args.Observed)
                    {
                        throw;
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }

        }
    }
}
