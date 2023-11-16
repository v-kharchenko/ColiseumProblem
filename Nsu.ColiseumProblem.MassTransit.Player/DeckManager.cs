using System.Runtime.ConstrainedExecution;

namespace Nsu.ColiseumProblem.MassTransit.Player
{
    public class DeckManager
    {
        private List<Card> _deck;
        private int _chosenCard;

        private bool _isCardNumberAvailable = false;
        private bool _isDeckAvailable = false;

        private TaskCompletionSource<bool> _deckCompletionSource = null!;
        private TaskCompletionSource<bool> _cardCompletionSource = null!;
        private object _deckLock = new();
        private object _cardLock = new();

        public void SetDeck(List<Card> deck)
        {
            lock (_deckLock) 
            {
                _deck = deck;
                if (!_isDeckAvailable)
                {
                    _isDeckAvailable = true;
                    _deckCompletionSource?.SetResult(true);
                }
            }
        }

        public void SetChosenCard(int cardNumber)
        {
            lock (_cardLock)
            {
                _chosenCard = cardNumber;
                if (!_isCardNumberAvailable)
                {
                    _isCardNumberAvailable = true;
                    _cardCompletionSource?.SetResult(true);
                }
            }
        }

        public async Task<CardColor> GetCardColorAsync()
        {
            await WaitForCard();
            await WaitForDeck();

            _isCardNumberAvailable = false;
            _isDeckAvailable = false;
            _cardCompletionSource = null!;
            _deckCompletionSource = null!;

            return _deck[_chosenCard].Color;
        }

        private Task WaitForCard()
        {
            lock (_cardLock)
            {
                if (_isCardNumberAvailable)
                {
                    return Task.CompletedTask;
                }
                
                _cardCompletionSource = new TaskCompletionSource<bool>();
            }

            return _cardCompletionSource.Task;
        }

        private Task WaitForDeck()
        {
            lock (_deckLock)
            {
                if (_isDeckAvailable)
                {
                    return Task.CompletedTask;
                }

                _deckCompletionSource = new TaskCompletionSource<bool>();
            }

            return _deckCompletionSource.Task;
        }
    }
}
