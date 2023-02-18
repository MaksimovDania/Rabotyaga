using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace umlaut
{
    public class HHruAPI
    {
        private readonly HttpClient _httpClient = new();
        public async Task<string> getRawPage(int pageNumber)
        {
            var rez = await SendGetRequest($"https://hh.ru/search/resume?university=38921&relocation=living_or_relocation&gender=unknown&text=&exp_period=all_time&logic=normal&no_magic=true&order_by=relevance&ored_clusters=true&pos=full_text&items_on_page=20&search_period=0&page={pageNumber}");
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

        public async Task<IEnumerable<string>> GetProfileHrefs(int num)
        {
            List<string> hrefTags = new List<string>();

            var parser = new HtmlParser();
            for (int i = 0; i < 250; i++)
            {
                var document = parser.ParseDocument(await getRawPage(i));
                foreach (IElement element in document.QuerySelectorAll("a"))
                {
                    if (element.GetAttribute("href").StartsWith("/resume/"))
                        hrefTags.Add(element.GetAttribute("href"));
                }
                Console.WriteLine(i);
            }
            

            return hrefTags;
        }
    }
}
