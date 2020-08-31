using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nba_ml_api.Models
{
    public class Team
    {
        public int Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
        
        public string Abbreviation { get; set; }

        public string NickName { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }

        public List<string> Seasons { get; set; }
        
        [JsonProperty("year_founded")]
        public int YearFounded { get; set; }
    }
}
