using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using System.Text.RegularExpressions;
using Umlaut.Database.Models;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using System.Net;
using System.Text.Json;
using Quickenshtein;
using Newtonsoft.Json;

namespace Umlaut
{
    public class HHruAPI
    {
        private HtmlParser parser = new HtmlParser();

        dynamic faculies;
        const int minDistance = 5;
        const int maxDistance = 35;
        public HHruAPI()
        {
            using (StreamReader reader = new StreamReader("faculties.json"))
            {
                string json = reader.ReadToEnd();
                faculies = JsonConvert.DeserializeObject(json);
            }
        }

        private readonly HttpClient _httpClient = new();
        public async Task<IHtmlDocument> getByAgePage(int age, int page)
        {
            var rez = await SendGetRequest($"https://hh.ru/search/resume?education_level=higher&university=38921&label=only_with_age&relocation=living_or_relocation&age_from={age}&age_to={age}&gender=unknown&text=&isDefaultArea=true&exp_period=all_time&logic=normal&pos=full_text&from=employer_index_header&search_period=&page={page}");
            return parser.ParseDocument(await rez.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> SendGetRequest(string requestString)
        {
            var req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(requestString);
            var rez = await _httpClient.SendAsync(req);
            if (!rez.IsSuccessStatusCode)
                throw new Exception("Защита от парсинга");
            return rez;
        }

        private async Task<IHtmlDocument> GetResume(string href)
        {
            var rez = await SendGetRequest($"https://hh.ru/resume/{href}");
            return parser.ParseDocument(await rez.Content.ReadAsStringAsync());
        }

        public async Task<Graduate> GetGraduate(string href)
        {
            try
            {
                var rez = new Graduate();
                rez.ResumeLink = href;
                var document = await GetResume(href);
                var title = document.QuerySelector("div.resume-header-title");
                rez.Gender = ParseGender(title.QuerySelector("span[data-qa='resume-personal-gender']").InnerHtml);
                rez.Age = int.Parse(title.QuerySelector("span[data-qa='resume-personal-age'] span").InnerHtml.Substring(0, 2));
                rez.Location = new Locations { Location = ParseLocation(title.QuerySelector("span[data-qa='resume-personal-address']").InnerHtml) };
                rez.ExpectedSalary = ParseSalary(document.QuerySelector("span.resume-block__salary"));
                rez.Experience = ParseExperience(document.QuerySelector("div.resume-block[data-qa='resume-block-experience'] h2 span"));
                rez.Vacation = document.QuerySelector("div.resume-block__title-text-wrapper h2  span.resume-block__title-text[data-qa='resume-block-title-position']").InnerHtml;
                rez.Specialization = document.QuerySelectorAll("li.resume-block__specialization").Select(a => new Specializations { Specialization = ParseSpecialization(a.InnerHtml) }).ToList();
                var bmstu = document.QuerySelectorAll("div.resume-block[data-qa='resume-block-education'] div.bloko-columns-row div.resume-block-item-gap")
                    .FirstOrDefault(a => a.InnerHtml.Contains("university=38921"));
                rez.YearGraduation = int.Parse(bmstu.QuerySelector("div.bloko-column_l-2").InnerHtml);
                var rawFac = bmstu.QuerySelector("div[data-qa='resume-block-education-organization']").InnerHtml.Replace(@"<!-- -->", "");
                rez.Faculty = new Faculties { Faculty = ClassificateFaculty(rawFac) };

                return FromChineese(rez);
            }
            catch (Exception ex)
            {
                using (StreamWriter mainText = File.AppendText("HHErrors.txt"))
                {
                    mainText.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                        mainText.WriteLine(ex.InnerException);
                    mainText.WriteLine(href);
                    mainText.WriteLine();
                }
            }
            return null;
        }
        private async Task<IEnumerable<string>> GetAllHrefsForAge(int age)
        {
            IEnumerable<string> links = new List<string>();
            Console.WriteLine(age);
            var document = await getByAgePage(age, 0);
            var batons = document.QuerySelectorAll("a.bloko-button span").Select(span => span.InnerHtml).ToList();
            links = links.Concat(document.QuerySelectorAll("a.serp-item__title").Select(elem => elem.GetAttribute("href").Substring(8, 38)));
            if (int.TryParse(batons[^2], out int pagesNumber))
            {
                for (int i = 1; i < pagesNumber; i++)
                {
                    document = await getByAgePage(age, i);
                    links = links.Concat(document.QuerySelectorAll("a.serp-item__title").Select(elem => elem.GetAttribute("href").Substring(8, 38)));
                }
            }
            return links;
        }

        public async Task<IEnumerable<string>> GetProfileHrefs()
        {
            IEnumerable<string> links = new List<string>();

            for (int i = 18; i < 23; i++)
            {
                links = links.Concat(await GetAllHrefsForAge(i));
            }
            return links;
        }

        private Graduate FromChineese(Graduate g)
        {
            g.Faculty.Faculty = UTF8ToWin1251(g.Faculty.Faculty).Replace("?", "");
            g.Gender = UTF8ToWin1251(g.Gender).Replace("?", ""); ;
            g.Location.Location = UTF8ToWin1251(g.Location.Location).Replace("?", "");
            g.Vacation = UTF8ToWin1251(g.Vacation).Replace("?", "");
            foreach (var spec in g.Specialization)
                spec.Specialization = UTF8ToWin1251(spec.Specialization).Replace("?", "");
            return g;
        }

        private string UTF8ToWin1251(string sourceStr)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("windows-1251");
            byte[] utf8Bytes = utf8.GetBytes(sourceStr);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            return win1251.GetString(win1251Bytes);
        }

        private string TranslateLocation(string loc) => loc switch
        {
            "Balashikha" => "Балашиха",
            "Batumi" => "Батуми",
            "Bulgaria" => "Болгария",
            "Egypt" => "Египет",
            "Fryazino" => "Фрязино",
            "Great Britain" => "Великобритания",
            "Israel" => "Израиль",
            "Kazan" => "Казань",
            "Krasnodar" => "Краснодар",
            "Moscow" => "Москва",
            "Saint Petersburg" => "Санкт-Петербург",
            "Sochi" => "Сочи",
            "USA" => "США",
            "Veliky Novgorod" => "Великий Новгород",
            _ => loc
        };

        private string ParseGender(string gen) => gen switch
        {
            "Мужчина" => gen,
            "Женщина" => gen,
            "Male" => "Мужчина",
            "Female" => "Женщина",
            _ => gen
        };

        private string ParseSpecialization(string str)
        {
            if (!Regex.IsMatch(str, "[а-яА-Я]"))
            {
                return TranslateSpecialization(str);
            }
            return str;
        }

        private string TranslateSpecialization(string spec) => spec switch
        {
            "Accountant" => "Бухгалтер",
            "Analyst" => "Аналитик",
            "Designer, artist" => "Дизайнер, художник",
            "Economist" => "Экономист",
            "Game designer" => "Гейм-дизайнер",
            "Head of production" => "Начальник производства",
            "Head of sales" => "Руководитель отдела продаж",
            "Information security specialist" => "Специалист по информационной безопасности",
            "Journalist, correspondent" => "Журналист, корреспондент",
            "System engineer" => "Системный инженер",
            "System administrator" => "Системный администратор",
            "Programmer, developer" => "Программист, разработчик",
            "Quality engineer" => "Инженер по качеству",
            "Sales manager, account manager" => "Менеджер по продажам, менеджер по работе с клиентами",
            "Tester" => "Тестировщик",
            "Data scientist" => "Дата-сайентист",
            _ => spec
        };

        private string ParseLocation(string str)
        {
            if (!Regex.IsMatch(str, "[а-яА-Я]"))
            {
                return TranslateLocation(str);
            }
            return str;
        }

        private int ParseSalary(AngleSharp.Dom.IElement salary)
        {
            if (salary == null) return 0;
            var str = salary.InnerHtml;
            double normal = int.Parse(Regex.Replace(str.Substring(0, str.IndexOf('<')), @"\s+", String.Empty));
            var currency = Regex.Match(str, @"(?<=\>)([^&\s]*?)(?=\<)").Value;
            return (int)(normal * GetExchangeRate(currency));
        }

        private Double GetExchangeRate(string currency) => currency switch
        {
            "руб." => 1,
            "RUB" => 1,
            "EUR" => 82.9,
            "USD" => 78,
            "AZN" => 45.27,
            "KZT" => 0.17,
            _ => 1,
        };

        private int ParseExperience(AngleSharp.Dom.IElement exp)
        {
            if (exp == null) return 0;
            var list = exp.QuerySelectorAll("span").Select(a => a.InnerHtml).ToList();
            if (list[0].Contains("years") || list[0].Contains("год") || list[0].Contains("годa") || list[0].Contains("лет"))
                return int.Parse(list[0].Substring(0, list[0].IndexOf("<")));
            else
                return int.Parse(list[0].Substring(0, list[0].IndexOf("<"))) > 5 ? 1 : 0;
        }

        public string ClassificateFaculty(string faculty)
        {
            int minLev = 10000;
            string title = faculty;
            foreach (dynamic fac in faculies)
            {
                foreach (string exaxtName in fac["Exact name"])
                {
                    if (faculty.Contains(exaxtName))
                        return fac.Title;
                }
                foreach (string possibleName in fac["Possible names"])
                {
                    int dist = Levenshtein.GetDistance(possibleName, faculty);
                    if (dist < minLev)
                    {
                        title = fac.Title;
                        minLev = dist;
                    }
                }
            }
            return UTF8ToWin1251(title).Replace("?", "");

        }
    }
}