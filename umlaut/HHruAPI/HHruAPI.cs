using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using DBModels;
using AngleSharp.Html.Dom;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace umlaut
{
    public class HHruAPI
    {
        private HtmlParser parser = new HtmlParser();

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

        private async Task<IHtmlDocument> GetRawResume(string href)
        {
            var rez = await SendGetRequest($"https://hh.ru/resume/{href}");
            return parser.ParseDocument(await rez.Content.ReadAsStringAsync());
        }

        public async Task<Graduate> GetGraduate(string href)
        {
            var document = await GetRawResume(href);
            var title = document.QuerySelector("div.resume-header-title");
            var gender = title.QuerySelector("span[data-qa='resume-personal-gender']").InnerHtml;
            var age = int.Parse(title.QuerySelector("span[data-qa='resume-personal-age'] span").InnerHtml.Substring(0, 2));
            var location = title.QuerySelector("span[data-qa='resume-personal-address']").InnerHtml;
            int salary;
            var salaryRaw = document.QuerySelector("span.resume-block__salary"); // проверть на нуль и запарсить
            if (salaryRaw is null)
                salary = 0;
            else
                salary = int.Parse(Regex.Replace(salaryRaw.InnerHtml.Substring(0, salaryRaw.InnerHtml.IndexOf('<')), @"\s+", String.Empty));

            int experience;
            var experienceRaw = document.QuerySelectorAll("span.resume-block__title-text_sub span").Select(a => a.InnerHtml).ToList();
            if (experienceRaw.Any())
                experience = int.Parse(experienceRaw[0].Substring(0, experienceRaw[0].IndexOf("<")));
            else
                experience = 0;

            var vacation = document.QuerySelector("div.resume-block__title-text-wrapper h2  span.resume-block__title-text[data-qa='resume-block-title-position']").InnerHtml;
            var specs = document.QuerySelectorAll("li.resume-block__specialization").Select(a => a.InnerHtml).ToList();
            var bmstu = document.QuerySelectorAll("div.resume-block[data-qa='resume-block-education'] div.bloko-columns-row div.resume-block-item-gap")
                                .FirstOrDefault(a => a.QuerySelector("a").InnerHtml.Contains("Баумана"));
            var year = int.Parse(bmstu.QuerySelector("div.bloko-column_l-2").InnerHtml);
            var Faculty = bmstu.QuerySelector("div[data-qa='resume-block-education-organization']").InnerHtml;

            return null;
        }
        public async Task<IEnumerable<string>> GetAllHrefsForAge(int age)
        {
            IEnumerable<string> links = new List<string>();
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

            for (int i = 20; i < 65; i++)
            {
                links = links.Concat(await GetAllHrefsForAge(i));
                Console.WriteLine(i);
            }

            return links;
        }


    }
}
