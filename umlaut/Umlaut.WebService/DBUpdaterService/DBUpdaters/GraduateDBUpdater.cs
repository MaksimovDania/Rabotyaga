using Umlaut.Database.Repositories.GraduateRepository;
using Umlaut.Database.Models;
using System.Text;

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
            var g = await _api.GetGraduate("f057927e000401f2cd0039ed1f593976667330");
            var flag = IsChineese(g);
            _repository.CreateGraduate(g);
            var hrefList = await _api.GetProfileHrefs();
            foreach (var href in hrefList)
            {
                if (!_repository.IsAlreadyExists(href))
                {
                    try
                    {
                        Console.WriteLine(href);
                        var graduate = await _api.GetGraduate(href);
                        if (graduate != null)
                        {
                            _repository.CreateGraduate(graduate);
                            Console.WriteLine($"{graduate.Vacation} added");
                        }
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter mainText = File.AppendText("DBErrors.txt"))
                        {
                            mainText.WriteLine(ex.Message);
                            if (ex.InnerException != null)
                                mainText.WriteLine(ex.InnerException);
                            mainText.WriteLine(href);
                            mainText.WriteLine();
                        }
                    }
                }
            }
        }

        private bool IsChineese(Graduate g)
        {
            var f = (g.ResumeLink == UTF8ToWin1251(g.ResumeLink));
            f = (g.Faculty.Faculty == UTF8ToWin1251(g.Faculty.Faculty));
            f = (g.Gender == UTF8ToWin1251(g.Gender));
            f = (g.Location.Location == UTF8ToWin1251(g.Location.Location));
            f = (g.Vacation == UTF8ToWin1251(g.Vacation));
            var rr = UTF8ToWin1251(g.Vacation);
            return (g.ResumeLink == UTF8ToWin1251(g.ResumeLink)) && (g.Faculty.Faculty == UTF8ToWin1251(g.Faculty.Faculty))
                && (g.Gender == UTF8ToWin1251(g.Gender)) && (g.Location.Location == UTF8ToWin1251(g.Location.Location))
                && (g.Vacation == UTF8ToWin1251(g.Vacation)) ? false : true;
        }

        private string UTF8ToWin1251(string sourceStr)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("windows-1251");
            byte[] utf8Bytes = utf8.GetBytes(sourceStr);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            return win1251.GetString(win1251Bytes);
        }
    }
}
