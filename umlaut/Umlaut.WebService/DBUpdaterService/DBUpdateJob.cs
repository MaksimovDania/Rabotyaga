using Quartz;
using System.Threading.Tasks;
using Umlaut.Database;
using Umlaut.WebService.DBUpdaterService.DBUpdaters;

namespace Umlaut.WebService.DBUpdaterService
{
    [DisallowConcurrentExecution]
    public class DBUpdateJob : IJob
    {
        private GraduateDBUpdater _updater;
        public DBUpdateJob(GraduateDBUpdater updater)
        {
            _updater = updater;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var t = new Task(async () =>
            {
               await _updater.Update();

            } );
            t.Start();
            return Task.CompletedTask;
        }
    }
}
