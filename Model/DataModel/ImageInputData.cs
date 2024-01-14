using Microsoft.ML.Transforms.Image;
using System.Drawing;

namespace Model.DataModel;

public class ImageInputData
{
    [ImageType(224, 224)]
    public Bitmap Image { get; set; }
}