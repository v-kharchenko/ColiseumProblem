using Nsu.ColiseumProblem.Sandbox;

namespace Nsu.ColiseumProblem.Web.Host
{
    public class ColiseumSandbox
    {
        private List<Card> _deck;
        private IDeckShufller _deckShufller;
        private CardMaster _cardMaster;
        private PlayerClient _playerClient;

        public ColiseumSandbox(
            IDeckShufller deckShufller,
            CardMaster cardMaster,
            PlayerClient playerClient)
        {
            _cardMaster = cardMaster;
            _deckShufller = deckShufller;
            _playerClient = playerClient;

            _deck = _cardMaster.CreateDefaultDeck36();
        }

        public async Task<bool> RunExperimentAsync()
        {
            _deckShufller.Shuffle(ref _deck);

            List<Card> elonDeck;
            List<Card> markDeck;

            _cardMaster.Split(in _deck, out elonDeck, out markDeck);

            int elonNumber = await _playerClient.SendDeckAsync(elonDeck, "Elon");
            int markNumber = await _playerClient.SendDeckAsync(markDeck, "Mark");

            Card elonChoiceCard = markDeck[elonNumber];
            Card markChoiceCard = elonDeck[markNumber];

            if (elonChoiceCard.Color == markChoiceCard.Color)
                return true;

            return false;
        }
    }
}