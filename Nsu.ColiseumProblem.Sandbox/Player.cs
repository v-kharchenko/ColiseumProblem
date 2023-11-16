namespace Nsu.ColiseumProblem.Sandbox
{
    public class Player
    {
        ICardPickStrategy _cardStrategy;

        public Player(ICardPickStrategy cardStrategy)
        {
            _cardStrategy = cardStrategy;
        }

        public int PickCard(List<Card> deck)
        {
            return _cardStrategy.PickCard(deck.ToArray());
        }
    }
}