using Microsoft.ML;
using Microsoft.ML.Data;
using nba_ml_api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nba_ml_api.ML
{
    public class GamePointsPredictionEngine : IGamePointsPredictionEngine
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "ML\\Data", "nba-team-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "ML\\Data", "nba-team-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "ML\\Data", "Model.zip");
        static ITransformer _trainedModel;
        static readonly MLContext _mlContext = new MLContext(seed: 0);

        public RegressionMetrics Metrics { get; set; }

        public GamePointsPredictionEngine()
        {
            //InitializeModel();
        }

        public GamePrediction PredictGamePoints(Game game)
        {
            var predictionFunction = _mlContext.Model.CreatePredictionEngine<Game, GamePrediction>(_trainedModel);

            return predictionFunction.Predict(game);
        }

        public GamePrediction PredictGamePoints(Game game, ITransformer model)
        {
            var predictionFunction = _mlContext.Model.CreatePredictionEngine<Game, GamePrediction>(model);

            return predictionFunction.Predict(game);
        }

        public ITransformer Train(string[] features, bool saveModel = false)
        {
            var defaultFeatures = new string[] { "TeamIdEncoded", "GameIdEncoded", "GameDateEncoded", "MatchUpEncoded", "OutcomeEncoded" };

            IDataView dataView = _mlContext.Data.LoadFromTextFile<Game>(_trainDataPath, hasHeader: true, separatorChar: ',');

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "TeamIdEncoded", inputColumnName: "TeamId")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "GameIdEncoded", inputColumnName: "GameId"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "GameDateEncoded", inputColumnName: "GameDate"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "MatchUpEncoded", inputColumnName: "MatchUp"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "OutcomeEncoded", inputColumnName: "Outcome"))
                .Append(_mlContext.Transforms.Concatenate("Features", defaultFeatures))
                .Append(_mlContext.Transforms.Concatenate("Features", features))
                .Append(_mlContext.Regression.Trainers.FastTree());

            var model = pipeline.Fit(dataView);

            if(saveModel)
                _mlContext.Model.Save(model, dataView.Schema, _modelPath);

            return model;
        }

        public RegressionMetrics Evaluate(ITransformer model)
        {
            IDataView dataView = _mlContext.Data.LoadFromTextFile<Game>(_testDataPath, hasHeader: true, separatorChar: ',');

            var predictions = model.Transform(dataView);

            return _mlContext.Regression.Evaluate(predictions, "Label", "Score");
        }

        private void InitializeModel()
        {
            var mlContext = new MLContext(seed: 0);
            if (File.Exists(_modelPath))
            {
                _trainedModel = mlContext.Model.Load(_modelPath, out DataViewSchema modelSchema);
            }
            else
            {
                var features = new string[] { "FGM", "FG3M", "FTM" };

                _trainedModel = Train(features, saveModel: true);
            }

            Metrics = Evaluate(_trainedModel);
        }
    }
}
