using Microsoft.AspNetCore.Mvc;

namespace Nsu.ColiseumProblem.Web.Player.Controllers
{
    [ApiController]
    [Route("player")]
    public class PlayerController : ControllerBase
    {
        private ICardPickStrategy _strategy;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger,
            ICardPickStrategy strategy)
        {
            _logger = logger;
            _strategy = strategy;
        }

        [HttpPost("pickcard")]
        public IActionResult PickCard([FromBody] List<Card> cardDeck)
        {
            int chosenCard = _strategy.PickCard(cardDeck.ToArray());
            
            return Ok(chosenCard);
        }

        private List<Card> GetDeckFromString(string stringDeck)
        {
            List<Card> cardDeck = new List<Card>();

            foreach (char color in stringDeck.ToLower())
            {
                if (color == 'r')
                    cardDeck.Add(new Card(CardColor.Red));
                else if (color == 'b')
                    cardDeck.Add(new Card(CardColor.Black));
            }

            return cardDeck;
        }
    }
}