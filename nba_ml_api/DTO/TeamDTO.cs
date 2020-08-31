using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nba_ml_api.DTO
{
    public class TeamDTO
    {
        public TeamDTO(string Id, string Year)
        {
            this.Id = Id;
            this.Year = Year;
        }

        public string Id { get; set; }
        public string Year { get; set; }
        public Dictionary<string, float> Stats { get; set; }
    }
}
