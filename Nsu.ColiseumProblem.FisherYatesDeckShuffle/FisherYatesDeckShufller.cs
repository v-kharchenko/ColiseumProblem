namespace Nsu.ColiseumProblem
{
    public class FisherYatesDeckShufller : IDeckShufller
    {
        public void Shuffle(ref List<Card> cards)
        {
            int n = cards.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }
    }
}