using Quartz;
using System.Threading.Tasks;


namespace umlaut.GraduateDBUpdaterBGService
{
    [DisallowConcurrentExecution]
    public class GraduateDBUpdateJob : IJob
    {
        private HHruAPI _api;
        public GraduateDBUpdateJob(HHruAPI api)
        {
            _api = api;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var t = new  Task(async ()=> Console.WriteLine((await _api.GetGraduate("bb5b392d0003daf91e009253956e434d373238")).Vacation));
            t.Start();
            return Task.CompletedTask;
        }
    }
}
