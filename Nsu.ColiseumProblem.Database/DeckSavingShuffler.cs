using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace Nsu.ColiseumProblem.Database
{
    public class DeckSavingShuffler : IDeckShufller
    {
        private IDeckShufller _deckShufflerImpl = new FisherYatesDeckShufller();
        private ColiseumContext _coliseumContext;

        public DeckSavingShuffler(ColiseumContext coliseumContext)
        {
            _coliseumContext = coliseumContext;
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            _coliseumContext.Cards.RemoveRange(_coliseumContext.Cards);
            _coliseumContext.Conditions.RemoveRange(_coliseumContext.Conditions);
            _coliseumContext.SaveChanges();
        }

        public void Shuffle(ref List<ColiseumProblem.Card> cards)
        {
            _deckShufflerImpl.Shuffle(ref cards);

            SaveConditionsToDatabase(cards);
        }

        private void SaveConditionsToDatabase(List<ColiseumProblem.Card> cards)
        {
            Condition condition = new Condition();

            for (int i = 0; i < cards.Count; i++)
            {
                Card card = new Card { Color = cards[i].Color.ToString(), DeckPosition = i };

                condition.Cards.Add(card);

                _coliseumContext.Cards.Add(card);
            }

            _coliseumContext.Conditions.Add(condition);

            _coliseumContext.SaveChanges();
        }
    }
}
