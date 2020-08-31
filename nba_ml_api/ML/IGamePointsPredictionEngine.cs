using Microsoft.ML;
using Microsoft.ML.Data;
using nba_ml_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nba_ml_api.ML
{
    public interface IGamePointsPredictionEngine
    {
        RegressionMetrics Metrics { get; set; }

        GamePrediction PredictGamePoints(Game game);

        GamePrediction PredictGamePoints(Game game, ITransformer model);

        ITransformer Train(string[] features, bool saveModel = false);

        RegressionMetrics Evaluate(ITransformer model);
    }
}