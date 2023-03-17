using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using AngleSharp.Html.Dom;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Umlaut.Database.Models;

namespace Umlaut
{
    public class HHruAPI
    {
        private HtmlParser parser = new HtmlParser();

        private readonly HttpClient _httpClient = new();
        public async Task<IHtmlDocument> getByAgePage(int age, int page)
        {
            var rez = await SendGetRequest($"https://hh.ru/search/resume?education_level=higher&education_level=unfinished_higher&university=38921&label=only_with_age&relocation=living_or_relocation&age_from={age}&age_to={age}&gender=unknown&text=&isDefaultArea=true&exp_period=all_time&logic=normal&pos=full_text&from=employer_index_header&search_period=&page={page}");
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
            var rez = new Graduate();
            rez.ResumeLink = href;
            var document = await GetResume(href);
            var title = document.QuerySelector("div.resume-header-title");
            var gender = title.QuerySelector("span[data-qa='resume-personal-gender']");
            rez.Gender = gender == null ? "Не указан" : gender.InnerHtml;
            var a = title.QuerySelector("span[data-qa='resume-personal-age'] span").InnerHtml;
            rez.Age = int.Parse(a.Substring(0, 2));
            rez.Location = new Locations { Location = title.QuerySelector("span[data-qa='resume-personal-address']").InnerHtml };

            var salaryRaw = document.QuerySelector("span.resume-block__salary");
            if (salaryRaw is null)
                rez.ExpectedSalary = 0;
            else
                rez.ExpectedSalary = int.Parse(Regex.Replace(salaryRaw.InnerHtml.Substring(0, salaryRaw.InnerHtml.IndexOf('<')), @"\s+", String.Empty));

            var experienceRaw = document.QuerySelectorAll("span.resume-block__title-text_sub span").Select(a => a.InnerHtml).ToList();
            if (experienceRaw.Any())
                rez.Experience = int.Parse(experienceRaw[0].Substring(0, experienceRaw[0].IndexOf("<"))); // проверить что не месяцев
            else
                rez.Experience = 0;

            rez.Vacation = document.QuerySelector("div.resume-block__title-text-wrapper h2  span.resume-block__title-text[data-qa='resume-block-title-position']").InnerHtml;
            rez.Specialization = document.QuerySelectorAll("li.resume-block__specialization").Select(a => new Specializations { Specialization = a.InnerHtml}).ToList();
            var bmstu = document.QuerySelectorAll("div.resume-block[data-qa='resume-block-education'] div.bloko-columns-row div.resume-block-item-gap").FirstOrDefault(a => a.InnerHtml.Contains("Баумана")); //часто падает с нулем System.NullReferenceException: "Object reference not set to an instance of an object."
            rez.YearGraduation = int.Parse(bmstu.QuerySelector("div.bloko-column_l-2").InnerHtml);
            var rawFac = bmstu.QuerySelector("div[data-qa='resume-block-education-organization']").InnerHtml.Replace(@"<!-- -->", "");
            rez.Faculty = new Faculties { Faculty = rawFac };

            return rez;
        }
        private async Task<IEnumerable<string>> GetAllHrefsForAge(int age)
        {
            IEnumerable<string> links = new List<string>();
            Console.WriteLine(age);
            var document = await getByAgePage(age, 0);
            var batons = document.QuerySelectorAll("a.bloko-button span").Select(span => span.InnerHtml).ToList();
            links = links.Concat(document.QuerySelectorAll("a.serp-item__title").Select(elem => elem.GetAttribute("href").Substring(8, 38)));
            if (int.TryParse(batons[^2], out  int pagesNumber))
            {
                for(int i = 1; i < pagesNumber; i++)
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

            //List<Task<IEnumerable<string>>> tasks = new();
            //for (int i = 20; i < 30; i++)
            //{
            //   tasks.Add(GetAllHrefsForAge(i));
            //}

            //IEnumerable<string>[] lists = await Task.WhenAll(tasks);

            //foreach (var list in lists)
            //{
            //    links = links.Concat(list);
            //}

            for (int i = 31; i < 33; i++)
            {
                links = links.Concat(await GetAllHrefsForAge(i));
            }


            return links;
        }


    }
}
