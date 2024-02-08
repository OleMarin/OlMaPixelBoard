using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace OlMaPixelBoard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        static readonly HttpClient client = new HttpClient();
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

            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.StatusCode);
                Console.WriteLine($"The Error isch: {response.StatusCode}");
            }
        }
    }
}
