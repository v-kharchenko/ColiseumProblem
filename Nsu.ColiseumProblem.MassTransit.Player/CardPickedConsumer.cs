using MassTransit;
using Nsu.ColiseumProblem.MassTransit.Entities;

namespace Nsu.ColiseumProblem.MassTransit.Player
{
    public class CardPickedConsumer :
        IConsumer<CardPicked>
    {
        private string _player;

        private DeckManager _deckManager;

        public CardPickedConsumer(IConfiguration configuration, DeckManager deckManager)
        {
            _player = configuration.GetValue<string>("player") ?? "public";
            _deckManager = deckManager;
        }

        public Task Consume(ConsumeContext<CardPicked> context)
        {
            if (context.Message.Player == _player)
                return Task.CompletedTask;

            _deckManager.SetChosenCard(context.Message.CardNumber);

            return Task.CompletedTask;
        }
    }
}
