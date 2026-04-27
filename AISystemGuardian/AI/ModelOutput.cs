using Microsoft.ML.Data;

namespace AISystemGuardian.AI
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string Prediction { get; set; }
    }
}
