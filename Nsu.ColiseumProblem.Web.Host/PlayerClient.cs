using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nsu.ColiseumProblem.Web.Host
{
    public class PlayerClient
    {
        private int _elonPort;
        private int _markPort;

        public PlayerClient(IConfiguration configuration)
        {
            _elonPort = configuration.GetValue<int>("elonport");
            _markPort = configuration.GetValue<int>("markport");
            Console.WriteLine(_elonPort + " " + _markPort);
        }



        public async Task<int> SendDeckAsync(List<Card> cards, string name)
        {
            using (HttpClient client = new HttpClient())
            {
                string json = JsonSerializer.Serialize(cards);

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                int port;
                if (name == "Elon")
                    port = _elonPort;
                else
                    port = _markPort;

                using HttpResponseMessage response = await client.PostAsync($"http://localhost:{port}/player/pickcard", content);

                response.EnsureSuccessStatusCode();

                int responseBody = Convert.ToInt32(await response.Content.ReadAsStringAsync());

                return responseBody;
            }
        }
    }
}
