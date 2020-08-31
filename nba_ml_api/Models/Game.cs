using System.Collections.Generic;
using Microsoft.ML.Data;

namespace nba_ml_api.Models
{
    public class Game
    {
        [LoadColumn(0)]
        public string TeamId { get; set; }

        [LoadColumn(1)]
        public string GameId { get; set; }

        [LoadColumn(2)]
        public string GameDate;

        [LoadColumn(3)]
        public string MatchUp;

        [LoadColumn(4)]
        public string Outcome { get; set; }

        [LoadColumn(5)]
        public string Min { get; set; }

        [LoadColumn(6)]
        public float FGM { get; set; }

        [LoadColumn(7)]
        public float FGA { get; set; }

        [LoadColumn(8)]
        public float FG_PCT { get; set; }

        [LoadColumn(9)]
        public float FG3M { get; set; }

        [LoadColumn(10)]
        public float FG3A { get; set; }

        [LoadColumn(11)]
        public float FG3_PCT { get; set; }

        [LoadColumn(12)]
        public float FTM { get; set; }

        [LoadColumn(13)]
        public float FTA { get; set; }

        [LoadColumn(14)]
        public float FT_PCT { get; set; }

        [LoadColumn(15)]
        public float OREB { get; set; }

        [LoadColumn(16)]
        public float DREB { get; set; }

        [LoadColumn(17)]
        public float REB { get; set; }

        [LoadColumn(18)]
        public float AST { get; set; }

        [LoadColumn(19)]
        public float STL { get; set; }

        [LoadColumn(20)]
        public float BLK { get; set; }

        [LoadColumn(21)]
        public float TOV { get; set; }

        [LoadColumn(22)]
        public float PF { get; set; }

        [LoadColumn(23)]
        [ColumnName("Label")]
        public float Points { get; set; }

        public List<KeyValuePair<string, object>> GetKeyValuePairs()
        {
            var retVal = new List<KeyValuePair<string, object>>();

            foreach (var prop in this.GetType().GetProperties())
            {
                var keyName = Feature.GetDisplayName(prop.Name);
                keyName = string.IsNullOrEmpty(keyName) ? prop.Name : keyName;
                retVal.Add(new KeyValuePair<string, object>(keyName, prop.GetValue(this)));
            }

            return retVal;
        }
    }

    public class GamePrediction
    {
        [ColumnName("Score")]
        public float Points;
    }
}
