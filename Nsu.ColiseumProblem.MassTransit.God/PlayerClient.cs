using MassTransit;
using Microsoft.Extensions.Configuration;
using Nsu.ColiseumProblem.MassTransit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nsu.ColiseumProblem.MassTransit.God
{
    public class PlayerClient
    {
        private int _port;
        private string _queue = "public-queue";

        private IConfiguration _configuration;
        private IBus _bus;

        public PlayerClient(IConfiguration configuration,
            IBus bus)
        {
            _configuration = configuration;
            _bus = bus;
        }

        internal void SetPlayer(string name)
        {
            name = name.ToLower();
            _port = _configuration.GetValue<int>(name.ToLower() + "port");
            _queue = name + "-queue";
        }

        public async void SendDeckAsync(List<Card> deck)
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/" + _queue));

            await endpoint.Send<PickCard>(new { Deck = deck });
        }

        public async Task<CardColor> GetColorAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using HttpResponseMessage response = await client.GetAsync($"http://localhost:{_port}/player/color");

                response.EnsureSuccessStatusCode();

                String responseBody = await response.Content.ReadAsStringAsync();

                CardColor color = Enum.Parse<CardColor>(responseBody);

                return color;
            }
        }
    }
}
