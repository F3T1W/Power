using Microsoft.ML.Data;

namespace Model.DataModel;

public class ImageLabelPredictions
{
    // TODO: should match TensorFlowModelSettings.outputTensorName
    [ColumnName("softmax2")]
    public float[] PredictedLabels { get; set; }
}