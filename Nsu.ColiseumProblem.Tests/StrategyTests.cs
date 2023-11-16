using Nsu.ColiseumProblem.Sandbox;
using Nsu.ColiseumProblem.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsu.ColiseumProblem.Tests
{
    internal class StrategyTests
    {
        private WinningCardStrategy _cardStrategy;
        private CardMaster _cardMaster;
        private FirstRedInPlaceNShuffler _deckShufller;

        private class FirstRedInPlaceNShuffler : IDeckShufller
        {
            public int FirstRedPosition { set; get; }

            public FirstRedInPlaceNShuffler()
            {
                FirstRedPosition = 0;
            }

            public void Shuffle(ref List<Card> cards)
            {
                cards.Clear();
                for (int i = 0; i < 36; i++)
                {
                    if (i != FirstRedPosition) cards.Add(new Card(CardColor.Black));
                    else cards.Add(new Card(CardColor.Red));
                }
            }
        }


        [SetUp]
        public void SetUp() 
        {
            _cardStrategy = new WinningCardStrategy();
            _cardMaster = new CardMaster();
            _deckShufller = new FirstRedInPlaceNShuffler();
        }

        [TestCase(5)]
        public void CardStrategy_WhenFirstRedCardIsInPlaceN_ReturnN(int firstRedPosition)
        {
            _deckShufller.FirstRedPosition = firstRedPosition;

            List<Card> deck = _cardMaster.CreateDefaultDeck36();

            _deckShufller.Shuffle(ref deck);

            int cardIndex = _cardStrategy.PickCard(deck.ToArray());

            Assert.That(cardIndex, Is.EqualTo(firstRedPosition));
        }
    }
}
