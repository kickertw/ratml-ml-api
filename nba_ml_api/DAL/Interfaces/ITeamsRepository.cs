using nba_ml_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nba_ml_api.DAL
{
    public interface ITeamsRepository : IRepository
    {
        Task<List<Team>> Get();

        Task<Dictionary<string, float>> GetAggregatedStats(string teamId, string year, string[] features);
    }
}
