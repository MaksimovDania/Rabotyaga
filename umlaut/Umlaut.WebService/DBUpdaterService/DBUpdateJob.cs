using Quartz;
using System.Threading.Tasks;
using Umlaut.Database;

namespace Umlaut.GraduateDBUpdaterBGService
{
    [DisallowConcurrentExecution]
    public class DBUpdateJob : IJob
    {
        private HHruAPI _api;
        private UmlautDBContext _context;
        public DBUpdateJob(HHruAPI api, UmlautDBContext context)
        {
            _api = api;
            _context = context;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var t = new Task(async () =>
            {
            var g = await _api.GetGraduate("efd05ff10007c1d23a0039ed1f627367636773");
                var a = _context.Faculties.Add(g.Faculty);
                _context.SaveChanges();
                Console.WriteLine(a);

            } );
            t.Start();
            return Task.CompletedTask;
        }
    }
}
