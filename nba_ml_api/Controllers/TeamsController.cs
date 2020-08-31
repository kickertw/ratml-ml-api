using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using nba_ml_api.DAL;
using nba_ml_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nba_ml_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsRepository _teamsRepository;

        public TeamsController(ITeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var teams = new List<Team>();

            teams = await _teamsRepository.Get();

            return Ok(teams);
        }
    }
}
