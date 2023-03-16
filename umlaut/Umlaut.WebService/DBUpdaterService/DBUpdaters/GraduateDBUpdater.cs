using Umlaut.Database.Repositories.GraduateRepository;
using Umlaut.Database.Models;

namespace Umlaut.WebService.DBUpdaterService.DBUpdaters
{
    public class GraduateDBUpdater : IDBUpdater
    {
        private HHruAPI _api;
        private IGraduateRepository _repository;

        public GraduateDBUpdater(HHruAPI api, IGraduateRepository repository)
        {
            _api = api;
            _repository = repository;
        }

        public async Task Update()
        {
            var hrefList = await _api.GetProfileHrefs();
            foreach (var href in hrefList)
            {
                if (!_repository.IsAlreadyExists(href))
                {
                    var graduate = await _api.GetGraduate(href);
                    _repository.CreateGraduate(graduate);
                    Console.WriteLine($"{graduate.Vacation} added");
                }
            }
        }
    }
}
