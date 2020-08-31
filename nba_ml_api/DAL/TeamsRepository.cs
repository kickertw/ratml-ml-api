using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using nba_ml_api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace nba_ml_api.DAL
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _nbaApiUrl;
        private readonly IMemoryCache _memoryCache;

        public TeamsRepository(IHttpClientFactory clientFactory, IConfiguration configuration, IMemoryCache memoryCache
            )
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _nbaApiUrl = _configuration["NbaApiUrl"];
            _memoryCache = memoryCache;
        }

        public async Task<List<Team>> Get()
        {
            var teams = new List<Team>();

            teams = await _memoryCache.GetOrCreateAsync(CacheKeys.Teams, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                var client = _clientFactory.CreateClient();

                var response = await client.GetAsync($"{_nbaApiUrl}/teams");

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    var tempTeams = JsonConvert.DeserializeObject<List<Team>>(responseStream);
                    tempTeams.ForEach(i => i.Seasons = GetSeasonsForTeam());

                    return tempTeams;
                }

                return new List<Team>();
            });

            return teams;
        }

        public async Task<Dictionary<string, float>> GetAggregatedStats(string teamId, string year, string[] features)
        {
            var stats = new Dictionary<string, float>();

            var client = _clientFactory.CreateClient();
            
            var queryString = $"{teamId}/{year}?";

            if (features != null && features.Any())
                queryString += $"features={string.Join(",", features)}";

            var response = await client.GetAsync($"{_nbaApiUrl}/aggregated-stats/{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                stats = JsonConvert.DeserializeObject<Dictionary<string, float>>(responseStream);
            }

            return stats;
        }

        // TODO: Figure out a better way to get available years
        private List<string> GetSeasonsForTeam()
        {
            var retVal = new List<string>();

            for (var ii = 1990; ii < DateTime.Now.Year; ii++)
            {
                retVal.Add($"{ii}-{(ii+1).ToString().Substring(2)}");
            }

            return retVal;
        }
    }
}
