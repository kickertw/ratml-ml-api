using nba_ml_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace nba_ml_api.DTO
{
    public class ForecastResultDTO
    {
        public int TeamAPoints { get; set; }
        public int TeamBPoints { get; set; }

        public List<KeyValuePair<string, object>> TeamAStats { get; set; }
        public List<KeyValuePair<string, object>> TeamBStats { get; set; }

        public RegressionMetrics Metrics { get; set; }

    }
}
