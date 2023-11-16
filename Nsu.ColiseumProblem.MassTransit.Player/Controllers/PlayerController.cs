using Microsoft.AspNetCore.Mvc;
using Nsu.ColiseumProblem.MassTransit.Player;

namespace Nsu.ColiseumProblem.Web.Player.Controllers
{
    [ApiController]
    [Route("player")]
    public class PlayerController : ControllerBase
    {
        private DeckManager _deckManager;

        public PlayerController(DeckManager deckManager)
        {
            _deckManager = deckManager;
        }

        [HttpGet("color")]
        public async Task<CardColor> GetColor()
        {
            CardColor color = await _deckManager.GetCardColorAsync();

            return await Task.FromResult(color);
        }
    }
}