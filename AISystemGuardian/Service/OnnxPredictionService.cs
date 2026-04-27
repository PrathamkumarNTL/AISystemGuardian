using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;

namespace AISystemGuardian.Service
{
    public class OnnxPredictionService
    {
        private readonly InferenceSession _session;

        public OnnxPredictionService(string modelPath)
        {
            _session = new InferenceSession(modelPath);
        }

        public string Predict(float cpu, float ram, float temp, float mic, float cam)
        {
            var input = new DenseTensor<float>(new[] { cpu, ram, temp, mic, cam }, new[] { 1, 5 });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", input)
            };

            using var results = _session.Run(inputs);

            var output = results.First().AsEnumerable<float>().ToArray();

            int predictedIndex = Array.IndexOf(output, output.Max());

            string[] labels = { "Normal", "Warning", "Critical" };

            return labels[predictedIndex];
        }
    }
}
