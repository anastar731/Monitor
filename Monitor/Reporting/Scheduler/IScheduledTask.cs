using System.Threading;
using System.Threading.Tasks;

namespace Monitor.Reporting.Scheduler
{
    public interface IScheduledTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
