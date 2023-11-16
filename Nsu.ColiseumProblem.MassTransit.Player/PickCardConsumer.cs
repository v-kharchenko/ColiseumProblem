using MassTransit;
using Nsu.ColiseumProblem.MassTransit.Entities;

namespace Nsu.ColiseumProblem.MassTransit.Player
{
    public class PickCardConsumer :
        IConsumer<PickCard>
    {
        private readonly ICardPickStrategy _strategy;
        private string _player;
        private DeckManager _deckManager;

        public PickCardConsumer(ICardPickStrategy strategy, IConfiguration configuration, DeckManager deckManager)
        {
            _strategy = strategy;
            _player = configuration.GetValue<string>("player") ?? "public";
            _deckManager = deckManager;
        }

        public Task Consume(ConsumeContext<PickCard> context)
        {
            List<Card> deck = context.Message.Deck;

            _deckManager.SetDeck(deck);

            int chosenCard = _strategy.PickCard(deck.ToArray());

            context.Publish<CardPicked>(new { Player = _player, CardNumber = chosenCard });

            return Task.CompletedTask;
        }
    }
}
