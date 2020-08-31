using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nba_ml_api.DTO
{
    public class GetWinningTeamDTO
    {
        public string TeamAId { get; set; }
        public string TeamBId { get; set; }
        public string TeamAYear{ get; set; }
        public string TeamBYear { get; set; }
        public string[] Features { get; set; }
    }
}
