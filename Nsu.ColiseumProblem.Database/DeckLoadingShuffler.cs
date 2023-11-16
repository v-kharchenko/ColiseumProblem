using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace Nsu.ColiseumProblem.Database
{
    public class DeckLoadingShuffler : IDeckShufller
    {
        private IDeckShufller _deckShufflerImpl = new FisherYatesDeckShufller();
        private ColiseumContext _coliseumContext;
        private Stack<Condition> _conditions;

        public DeckLoadingShuffler(ColiseumContext coliseumContext)
        {
            _coliseumContext = coliseumContext;
            _conditions = new Stack<Condition>(_coliseumContext.Conditions.ToArray());
        }

        public void Shuffle(ref List<ColiseumProblem.Card> cards)
        {
            if (_conditions.Peek() != null)
            {
                loadCondition(ref cards);
            }
            else
            {
                _deckShufflerImpl.Shuffle(ref cards);
            }
        }

        private void loadCondition(ref List<ColiseumProblem.Card> cards)
        {
            Condition condition = _conditions.Pop();
            string[] colors = _coliseumContext.Cards.Where(x => x.Condition.Id == condition.Id).Select(x => x.Color).ToArray();
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i] = new ColiseumProblem.Card(Enum.Parse<CardColor>(colors[i]));
            }
        }
    }
}
