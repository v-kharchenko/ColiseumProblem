namespace Nsu.ColiseumProblem.Sandbox
{
    public class ColiseumSandbox
    {
        private List<Card> _deck;
        private IDeckShufller _deckShufller;
        private CardMaster _cardMaster;

        private Player _elonMusk;
        private Player _markZuckerberg;

        public ColiseumSandbox(
            IDeckShufller deckShufller,
            CardMaster cardMaster,
            Player elonMusk,
            Player markZuckerberg)
        {
            _cardMaster = cardMaster;
            _deckShufller = deckShufller;
            _elonMusk = elonMusk;
            _markZuckerberg = markZuckerberg;

            _deck = _cardMaster.CreateDefaultDeck36();
        }

        public bool RunExperiment()
        {
            _deckShufller.Shuffle(ref _deck);

            List<Card> elonDeck;
            List<Card> markDeck;

            _cardMaster.Split(in _deck, out elonDeck, out markDeck);

            int elonNumber = _elonMusk.PickCard(elonDeck);
            int markNumber = _markZuckerberg.PickCard(markDeck);

            Card elonChoiceCard = markDeck[elonNumber];
            Card markChoiceCard = elonDeck[markNumber];

            if (elonChoiceCard.Color == markChoiceCard.Color)
                return true;

            return false;
        }
    }
}