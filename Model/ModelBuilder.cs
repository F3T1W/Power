﻿using Microsoft.ML;
using Model.DataModel;
using System.Collections.Generic;
using System.Linq;

namespace Model;

public class Model(string tensorFlowModelFilePath, string mlnetOutputZipFilePath)
{
    private readonly string _tensorFlowModelFilePath = tensorFlowModelFilePath;
    private readonly string _mlnetOutputZipFilePath = mlnetOutputZipFilePath;

    public void Run()
    {
        // Create new model context
        var mlContext = new MLContext();

        // Define the model pipeline:
        //    1. loading and resizing the image
        //    2. extracting image pixels
        //    3. running pre-trained TensorFlow model
        var pipeline = mlContext.Transforms.ResizeImages(
                outputColumnName: TensorFlowModelSettings.inputTensorName,
                imageWidth: ImageSettings.imageWidth,
                imageHeight: ImageSettings.imageHeight,
                inputColumnName: nameof(ImageInputData.Image)
            )
            .Append(mlContext.Transforms.ExtractPixels(
                outputColumnName: TensorFlowModelSettings.inputTensorName,
                interleavePixelColors: ImageSettings.interleavePixelColors,
                offsetImage: ImageSettings.offsetImage)
            )
            .Append(mlContext.Model.LoadTensorFlowModel(_tensorFlowModelFilePath)
                .ScoreTensorFlowModel(
                    outputColumnNames: [TensorFlowModelSettings.outputTensorName],
                    inputColumnNames: [TensorFlowModelSettings.inputTensorName],
                    addBatchDimensionInput: true));

        // Train the model
        // Since we are simply using a pre-trained TensorFlow model, we can "train" it against an empty dataset
        var emptyTrainingSet = mlContext.Data.LoadFromEnumerable(new List<ImageInputData>());
        ITransformer mlModel = pipeline.Fit(emptyTrainingSet);

        // Save/persist the model to a .ZIP file
        // This will be loaded into a PredictionEnginePool by the Blazor application, so it can classify new images
        mlContext.Model.Save(mlModel, null, _mlnetOutputZipFilePath);
    }

    public struct ImageSettings
    {
        // This has to match how the pretrained model was trained
        // Reusing the same settings from where it was downloaded: https://github.com/dotnet/machinelearning-samples/tree/master/samples/csharp/getting-started/DeepLearning_ImageClassification_TensorFlow
        public const int imageHeight = 224;
        public const int imageWidth = 224;
        public const float offsetImage = 117;
        public const bool interleavePixelColors = true;
    }

    // For checking tensor names, you can open the TF model .pb file with tools like Netron: https://github.com/lutzroeder/netron
    // which is also available online https://lutzroeder.github.io/netron/
    public struct TensorFlowModelSettings
    {
        // input tensor name
        public const string inputTensorName = "input";

        // output tensor name
        public const string outputTensorName = "softmax2";
    }
}