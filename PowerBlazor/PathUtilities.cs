using System.IO;

namespace PowerBlazor;

public class PathUtilities
{
public static string GetPathFromBinFolder(string relativePath)
{
    FileInfo _dataRoot = new(typeof(Program).Assembly.Location);
    string assemblyFolderPath = _dataRoot.Directory.FullName;

    string fullPath = Path.Combine(assemblyFolderPath, relativePath);
    return fullPath;
}
}