using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using nba_ml_api.ML;

namespace nba_ml_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly IGamePointsPredictionEngine _gamePointsPredictionEngine;

        public TrainController(IGamePointsPredictionEngine gamePointsPredictionEngine)
        {
            _gamePointsPredictionEngine = gamePointsPredictionEngine;
        }

        [HttpPost]
        public IActionResult TrainModel(string[] features)
        {
            ITransformer model = _gamePointsPredictionEngine.Train(features);

            RegressionMetrics metrics = _gamePointsPredictionEngine.Evaluate(model);

            return Ok(metrics);
        }
    }
}
