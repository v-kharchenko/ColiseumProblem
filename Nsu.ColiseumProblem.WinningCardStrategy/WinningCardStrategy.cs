namespace Nsu.ColiseumProblem.Strategy
{
    public class WinningCardStrategy : ICardPickStrategy
    {
        public int PickCard(Card[] cards)
        {
            int index = 0;
            CardColor color = CardColor.Black;

            while (color.Equals(CardColor.Black) && index < cards.Length)
            {
                color = cards[index++].Color;
            }

            index--;

            return index;
        }
    }
}