using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nba_ml_api.Models;

namespace nba_ml_api.Controllers
{
    /// <summary>
    /// Used to retrieve list of possible features to send to the API
    /// to train the ML model.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        // static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "ML\\Data", "nba-team-train.csv");

        private static readonly string teamFeaturesKeys =
            "FGM,FGA,FG_PCT,FG3M,FG3A,FG3_PCT,FTM,FTA,FT_PCT,OREB,DREB,REB,AST,STL,BLK,TOV,PF,PTS";

        [HttpGet]
        [Route("teams")]
        public IActionResult GetTeamPredictionFeatures()
        {
            var features = GetTeamFeatures();
            return Ok(features);
        }

        private List<Feature> GetTeamFeatures()
        {
            var retVal = new List<Feature>();
            foreach (var key in teamFeaturesKeys.Split(','))
            {
                var feature = new Feature
                {
                    Key = key,
                    Display = Feature.GetDisplayName(key)
                };

                if (!string.IsNullOrEmpty(feature.Display))
                {
                    retVal.Add(feature);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Method that can be used to quickly get features initally. Won't be called as the data schema/structure does not change often
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ReadCsvHeaders(string path)
        {
            return System.IO.File.ReadLines(path).First();
        }
    }
}
