using System;
using System.IO;

namespace Model;

class Program
{
    static void Main()
    {
        var tensorFlowModelPath = "TFInceptionModel/tensorflow_inception_graph.pb";
        var mlnetOutputZipFilePath = "PredictionModel.zip";
        var Model = new Model(tensorFlowModelPath, mlnetOutputZipFilePath);
        Model.Run();

        Console.WriteLine($"Generated {Path.GetFullPath(mlnetOutputZipFilePath)}");
    }
}
