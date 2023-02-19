using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;

namespace umlaut
{
    public class HHruAPI
    {
        private HtmlParser parser = new HtmlParser();

        private readonly HttpClient _httpClient = new();
        public async Task<string> getByAgePage(int age, int page)
        {
            var rez = await SendGetRequest($"https://hh.ru/search/resume?education_level=higher&university=38921&label=only_with_age&relocation=living_or_relocation&age_from={age}&age_to={age}&gender=unknown&text=&isDefaultArea=true&exp_period=all_time&logic=normal&pos=full_text&from=employer_index_header&search_period=&page={page}");
            return await rez.Content.ReadAsStringAsync();
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

        private async Task<string> GetRawVacation(string href)
        {
            var rez = await SendGetRequest($"https://hh.ru/resume/{href}");
            return await rez.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<string>> GetAllHrefsForAge(int age)
        {
            IEnumerable<string> links = new List<string>();
            var document = parser.ParseDocument(await getByAgePage(age, 0));
            var batons = document.QuerySelectorAll("a.bloko-button span").Select(span => span.InnerHtml).ToList();
            links = links.Concat(document.QuerySelectorAll("a.serp-item__title").Select(elem => elem.GetAttribute("href").Substring(8, 38)));
            if (int.TryParse(batons[^2], out  int pagesNumber))
            {
                for(int i = 1; i < pagesNumber; i++)
                {
                    document = parser.ParseDocument(await getByAgePage(age, i));
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
