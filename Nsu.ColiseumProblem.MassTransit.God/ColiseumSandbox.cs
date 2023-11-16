using Nsu.ColiseumProblem.MassTransit.God;
using Nsu.ColiseumProblem.Sandbox;

namespace Nsu.ColiseumProblem.MassTransit.God
{
    public class ColiseumSandbox
    {
        private List<Card> _deck;
        private IDeckShufller _deckShufller;
        private CardMaster _cardMaster;
        private PlayerClient _elonClient;
        private PlayerClient _markClient;

        public ColiseumSandbox(
            IDeckShufller deckShufller,
            CardMaster cardMaster,
            PlayerClient elonClient,
            PlayerClient markClient)
        {
            _cardMaster = cardMaster;
            _deckShufller = deckShufller;

            _elonClient = elonClient;
            _elonClient.SetPlayer("Elon");

            _markClient = markClient;
            _markClient.SetPlayer("Mark");

            _deck = _cardMaster.CreateDefaultDeck36();
        }

        public async Task<bool> RunExperimentAsync()
        {
            _deckShufller.Shuffle(ref _deck);

            _cardMaster.Split(in _deck, out List<Card> elonDeck, out List<Card> markDeck);

            _elonClient.SendDeckAsync(elonDeck);
            _markClient.SendDeckAsync(markDeck);

            CardColor elonColor = await _elonClient.GetColorAsync();
            CardColor markColor = await _markClient.GetColorAsync();

            if (elonColor == markColor)
                return true;
            

            return false;
        }
    }
}