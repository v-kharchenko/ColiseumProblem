namespace Nsu.ColiseumProblem.Sandbox
{
    public class CardMaster
    {
        public List<Card> CreateDefaultDeck36()
        {
            List<Card> cards = new List<Card>();

            for (int number = 0; number < 18; number++)
            {
                cards.Add(new Card(CardColor.Black));
                cards.Add(new Card(CardColor.Red));
            }

            return cards;
        }

        public void Split(in List<Card> inDeck, out List<Card> outDeck1, out List<Card> outDeck2)
        {
            outDeck1 = inDeck.GetRange(0, inDeck.Count / 2 + inDeck.Count % 2);
            outDeck2 = inDeck.GetRange(inDeck.Count / 2 + inDeck.Count % 2, inDeck.Count / 2);
        }
    }
}
