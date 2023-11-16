using Nsu.ColiseumProblem.Sandbox;

namespace Nsu.ColiseumProblem.Tests
{
    public class TestDeck
    {
        private CardMaster _cardMaster;

        [SetUp]
        public void Setup()
        {
            _cardMaster = new CardMaster();
        }

        [Test]
        public void CardMaster_CreateDefaultDeck36_Has18BlackAnd18RedCards()
        {
            List<Card> deck = _cardMaster.CreateDefaultDeck36();
            
            int blackCardsCount = 0;
            int redCardsCount = 0;
            
            foreach (var card in deck)
            {
                if (card.Color == CardColor.Black) blackCardsCount++;
                if (card.Color == CardColor.Red) redCardsCount++;
            }

            Assert.That(blackCardsCount, Is.EqualTo(18));
            Assert.That(redCardsCount, Is.EqualTo(18));
        }
    }
}
