using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace OlMaPixelBoard.Pages
{
    public class IndexModel : PageModel
    {
        static readonly HttpClient client = new HttpClient();
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        public string BoardApiUrl;
        public string testPixel;

        public int[,] reds = new int[16, 16];
        public int[,] bleus = new int[16, 16];
        public int[,] greens = new int[16, 16];

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            var tasks = new List<Task>();

            try
            {

                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        string content = await client.GetStringAsync("http://localhost:5085/api/color/" + y + "/" + x);

                        Console.WriteLine("Success" + content);
                        testPixel = content;


                        var match = Regex.Match(content, "\"Red\":(.*),\"Green\":(.*),\"Blue\":(.*)}");

                        if (content != "")
                        {
                            reds[y, x] = Convert.ToInt32(match.Groups[1].Value);

                            greens[y, x] = Convert.ToInt32(match.Groups[2].Value);

                            bleus[y, x] = Convert.ToInt32(match.Groups[3].Value);
                        }
                        else
                        {
                            reds[y, x] = 255;

                            greens[y, x] = 255;

                            bleus[y, x] = 255;
                        }

                    }

                }
                return Page();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return BadRequest();
            }
        }

        public async Task<ActionResult> BuildSite()
        {
            var tasks = new List<Task>();

            try
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        HttpResponseMessage response = client.GetAsync(/*"https://edu.jakobmeier.ch/api/color/"*/ "http://localhost:5085/api/color/" + y + "/" + x).Result;



                        string content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("Success" + content);
                        testPixel = content;


                        var match = Regex.Match(content, "\"Red\":(.*),\"Green\":(.*),\"Blue\":(.*)}");

                        if (response != null)
                        {
                            reds[y, x] = Convert.ToInt32(match.Groups[1].Value);

                            greens[y, x] = Convert.ToInt32(match.Groups[2].Value);

                            bleus[y, x] = Convert.ToInt32(match.Groups[3].Value);
                        }
                        else
                        {
                            reds[y, x] = 255;

                            greens[y, x] = 255;

                            bleus[y, x] = 255;
                        }
                    }
                }

                return Page();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return BadRequest();
            }

        }

        public async Task<IActionResult> OnPostAsync(int postValueX, int postValueY)
        {
            try
            {
                var postContent = new { X = postValueX, Y = postValueY, Team = 2 };
                var postJson = JsonConvert.SerializeObject(postContent);
                var postResponse = await client.PostAsync($"http://localhost:5085/api/color", new StringContent(postJson, System.Text.Encoding.UTF8, "application/json"));
                postResponse.EnsureSuccessStatusCode();
                return await BuildSite();
            }
            catch
            {
                Console.WriteLine("\nException Caught!");
                return BadRequest();
            }
        }

        public class Colors
        {
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }
        }
    }
}
