using Nsu.ColiseumProblem.Sandbox;
using Nsu.ColiseumProblem.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsu.ColiseumProblem.Tests
{
    internal class SandboxTests
    {
        private class DeckShufflerCounter : IDeckShufller
        {
            private FisherYatesDeckShufller _deckShuffler = new FisherYatesDeckShufller();
            public int TimesShuffled { get; private set; } = 0;

            public void Shuffle(ref List<Card> cards)
            {
                _deckShuffler.Shuffle(ref cards);
                TimesShuffled++;
            }
        }

        private class FirstCardStrategy : ICardPickStrategy
        {
            public int PickCard(Card[] cards)
            {
                return 0;
            }
        }

        private class UnmatchingFirstCardsShuffler : IDeckShufller
        {
            public void Shuffle(ref List<Card> cards)
            {
                cards[0] = new Card(CardColor.Black);
                cards[cards.Count / 2] = new Card(CardColor.Red);
            }
        }

        [Test]
        public void DeckShuffler_WhenSandboxExperimentRuns_isCalledOnce()
        {
            DeckShufflerCounter deckShuffler = new DeckShufflerCounter();
            CardMaster cardMaster = new CardMaster();
            ICardPickStrategy cardStrategy = new WinningCardStrategy();
            Player elon = new Player(cardStrategy);
            Player mark = new Player(cardStrategy);

            ColiseumSandbox sandbox = new ColiseumSandbox(deckShuffler, cardMaster, elon, mark);

            sandbox.RunExperiment();

            Assert.That(deckShuffler.TimesShuffled, Is.EqualTo(1));
        }

        [Test]
        public void ColiseumSandbox_WithPredefinedInput_ReturnsExpected()
        {
            CardMaster cardMaster = new CardMaster();
            IDeckShufller deckShuffler = new UnmatchingFirstCardsShuffler();
            ICardPickStrategy cardStrategy = new FirstCardStrategy();
            
            Player elon = new Player(cardStrategy);
            Player mark = new Player(cardStrategy);

            ColiseumSandbox sandbox = new ColiseumSandbox(deckShuffler, cardMaster, elon, mark);

            bool didColorsMatch = sandbox.RunExperiment();

            Assert.IsFalse(didColorsMatch);
        }
    }
}
