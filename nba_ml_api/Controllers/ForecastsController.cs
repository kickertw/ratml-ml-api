using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.ML;
using Microsoft.ML.Data;
using nba_ml_api.DAL;
using nba_ml_api.DTO;
using nba_ml_api.ML;
using nba_ml_api.Models;

namespace nba_ml_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ForecastsController : ControllerBase
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IGamePointsPredictionEngine _gamePointsPredictionEngine;

        public ForecastsController(ITeamsRepository teamsRepository, IGamePointsPredictionEngine gamePointsPredictionEngine)
        {
            _teamsRepository = teamsRepository;
            _gamePointsPredictionEngine = gamePointsPredictionEngine;
        }

        [HttpPost]
        [Route("/forecasts/teams")]
        public async Task<IActionResult> GetWinningTeam(GetWinningTeamDTO dto)
        {
            Game teamAStats;

            Game teamBStats;

            if (dto.TeamAFeatures != null && dto.TeamBFeatures != null
                && dto.TeamAFeatures.Any() && dto.TeamBFeatures.Any())
            {
                teamAStats = GetGameStats(dto.TeamAId, dto.TeamAYear, dto.TeamAFeatures);

                teamBStats = GetGameStats(dto.TeamBId, dto.TeamBYear, dto.TeamBFeatures);
            }

            else
            {
                teamAStats = await GetGameStats(dto.TeamAId, dto.TeamAYear, dto.Features);

                teamBStats = await GetGameStats(dto.TeamBId, dto.TeamBYear, dto.Features);
            }

            ITransformer model = _gamePointsPredictionEngine.Train(dto.Features);

            RegressionMetrics metrics = _gamePointsPredictionEngine.Evaluate(model);

            var forecast = new ForecastResultDTO
            {
                Metrics = metrics,
                TeamAStats = teamAStats.GetKeyValuePairs(),
                TeamBStats = teamBStats.GetKeyValuePairs()
            };

            forecast.TeamAPoints = (int)_gamePointsPredictionEngine.PredictGamePoints(teamAStats, model).Points;

            forecast.TeamBPoints = (int)_gamePointsPredictionEngine.PredictGamePoints(teamBStats, model).Points;

            return Ok(forecast);
        }

        private Game GetGameStats(string teamId, string teamYear, Dictionary<string, float> userFeatures)
        {
            return new Game
            {
                TeamId = teamId,
                GameDate = teamYear,
                AST = GetStat("AST", userFeatures),
                BLK = GetStat("BLK", userFeatures),
                DREB = GetStat("DREB", userFeatures),
                FG3A = GetStat("FG3A", userFeatures),
                FG3M = GetStat("FG3M", userFeatures),
                FG3_PCT = GetStat("FG3_PCT", userFeatures),
                FGA = GetStat("FGA", userFeatures),
                FGM = GetStat("FGM", userFeatures),
                FG_PCT = GetStat("FG_PCT", userFeatures),
                FTA = GetStat("FTA", userFeatures),
                FTM = GetStat("FTM", userFeatures),
                FT_PCT = GetStat("FT_PCT", userFeatures),
                OREB = GetStat("OREB", userFeatures),
                PF = GetStat("PF", userFeatures),
                REB = GetStat("REB", userFeatures),
                STL = GetStat("STL", userFeatures),
                TOV = GetStat("TOV", userFeatures)
            };
        }

        private async Task<Game> GetGameStats(string teamId, string teamYear, string[] features)
        {
            var team = new TeamDTO(teamId, teamYear);

            team.Stats = await _teamsRepository.GetAggregatedStats(team.Id, team.Year, features);

            return new Game
            {
                TeamId = team.Id,
                GameDate = team.Year,
                AST = GetStat("AST", team.Stats),
                BLK = GetStat("BLK", team.Stats),
                DREB = GetStat("DREB", team.Stats),
                FG3A = GetStat("FG3A", team.Stats),
                FG3M = GetStat("FG3M", team.Stats),
                FG3_PCT = GetStat("FG3_PCT", team.Stats),
                FGA = GetStat("FGA", team.Stats),
                FGM = GetStat("FGM", team.Stats),
                FG_PCT = GetStat("FG_PCT", team.Stats),
                FTA = GetStat("FTA", team.Stats),
                FTM = GetStat("FTM", team.Stats),
                FT_PCT = GetStat("FT_PCT", team.Stats),
                OREB = GetStat("OREB", team.Stats),
                PF = GetStat("PF", team.Stats),
                REB = GetStat("REB", team.Stats),
                STL = GetStat("STL", team.Stats),
                TOV = GetStat("TOV", team.Stats)
            };
        }

        private float GetStat(string property, Dictionary<string, float> stats)
        {
            stats.TryGetValue(property, out float stat);
            
            return stat;
        }
    }
}
