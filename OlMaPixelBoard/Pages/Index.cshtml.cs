using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace OlMaPixelBoard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        static readonly HttpClient client = new HttpClient();
        public string testPixel;
        public int red;
        public int green;
        public int bleu;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            HttpResponseMessage response = client.GetAsync("https://edu.jakobmeier.ch/api/color/0/4").Result;

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Success" + content);
                testPixel = content;


                var match = Regex.Match(content, "\"Red\":(.*),\"Green\":(.*),\"Blue\":(.*)}");   
               
                red = Convert.ToInt32(match.Groups[1].Value);

                green = Convert.ToInt32(match.Groups[2].Value);

                bleu = Convert.ToInt32(match.Groups[3].Value);


            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.StatusCode);
                Console.WriteLine($"Da Error is: {response.StatusCode}");
            }
        }
    }
}
